using MelonLoader;

namespace RallyXAutoSplitter.Adapters;

public interface ILogger
{
    void Msg(string message);
    void Warning(string message);
    void Error(string message);
}

public class MelonLoggerAdapter : ILogger
{
    private readonly MelonLogger.Instance _logger;

    public MelonLoggerAdapter(MelonLogger.Instance logger)
    {
        _logger = logger;
    }

    public void Msg(string message) => _logger.Msg(message);
    public void Warning(string message) => _logger.Warning(message);
    public void Error(string message) => _logger.Error(message);
}
