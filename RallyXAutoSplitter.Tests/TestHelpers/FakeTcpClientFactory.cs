using RallyXAutoSplitter.Services;
using System.Net.Sockets;

namespace RallyXAutoSplitter.Tests.TestHelpers;

public class FakeTcpClientFactory : ITcpClientFactory
{
    public bool ShouldConnect { get; set; }

    public ITcpClientWrapper CreateClient()
    {
        return new FakeTcpClient(ShouldConnect);
    }
}

public class FakeTcpClient : ITcpClientWrapper
{
    private readonly bool _shouldConnect;
    private readonly MemoryStream _stream = new();

    public FakeTcpClient(bool shouldConnect)
    {
        _shouldConnect = shouldConnect;
    }

    public bool Connected => _shouldConnect;

    public void Connect(string host, int port)
    {
        if (!_shouldConnect)
        {
            throw new SocketException();
        }
    }

    public Stream GetStream()
    {
        return _stream;
    }

    public void Dispose()
    {
        _stream?.Dispose();
    }
}
