using RallyXAutoSplitter.Adapters;

namespace RallyXAutoSplitter.Services;

public interface ILiveSplitService
{
    void StartTimer();
    void Split();
    void ResetRun();
}
public interface ITcpClientFactory
{
    ITcpClientWrapper CreateClient();
}

public interface ITcpClientWrapper : IDisposable
{
    bool Connected { get; }
    void Connect(string host, int port);
    Stream GetStream();
}


public class LiveSplitService : ILiveSplitService, IDisposable
{
    private readonly ILogger _logger;
    private readonly ITcpClientFactory _tcpFactory;

    private ITcpClientWrapper? _client;
    private StreamWriter? _writer;

    private bool _hasWarnedConnectionFailure;

    public LiveSplitService(ILogger logger, ITcpClientFactory tcpFactory)
    {
        _logger = logger;
        _tcpFactory = tcpFactory;
    }

    private readonly SemaphoreSlim _sendLock = new(1, 1);

    public void StartTimer()
    {
        _logger.Msg("[AUTOSPLITTER] START TIMER");
        SendAsync("starttimer");
    }

    public void Split()
    {
        _logger.Msg("[AUTOSPLITTER] SPLIT");
        SendAsync("split");
    }

    public void ResetRun()
    {
        _logger.Msg("[AUTOSPLITTER] RESET");
        SendAsync("reset");
    }

    private void SendAsync(string command)
    {
        // Fire and forget - don't block the game thread
        // Use a semaphore to ensure commands are sent in order
        Task.Run(async () =>
        {
            try
            {
                await _sendLock.WaitAsync();
                try
                {
                    Send(command);
                }
                finally
                {
                    _sendLock.Release();
                }
            }
            catch (Exception ex)
            {
                _logger.Warning($"Background send failed for '{command}': {ex.Message}");
            }
        });
    }

    private void Send(string command)
    {
        if (!EnsureConnected())
            return;

        try
        {
            _logger.Msg($"-> LiveSplit: {command}");
            _writer!.WriteLine(command);
        }
        catch (Exception ex)
        {
            _logger.Warning($"Failed sending '{command}': {ex.Message}");
        }
    }

    public bool EnsureConnected()
    {
        try
        {
            if (_client?.Connected == true)
                return true;

            _client?.Dispose();

            _client = _tcpFactory.CreateClient();

            // Use a short connection timeout to avoid blocking
            var connectTask = Task.Run(() => _client.Connect("127.0.0.1", 16834));
            if (!connectTask.Wait(TimeSpan.FromSeconds(2)))
            {
                throw new TimeoutException("Connection timeout");
            }

            _writer = new StreamWriter(_client.GetStream())
            {
                AutoFlush = true
            };

            if (_hasWarnedConnectionFailure)
            {
                _logger.Msg("Connected to LiveSplit.");
            }

            _hasWarnedConnectionFailure = false;

            return true;
        }
        catch
        {
            if (!_hasWarnedConnectionFailure)
            {
                _logger.Warning(
                    "Could not connect to LiveSplit. Is the TCP server running?");

                _hasWarnedConnectionFailure = true;
            }

            return false;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _sendLock?.Dispose();
        _writer?.Dispose();
        _client?.Dispose();
    }
}