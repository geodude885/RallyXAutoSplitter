using RallyXAutoSplitter.Services;
using System.Net.Sockets;

namespace RallyXAutoSplitter.Adapters;

public class TcpClientFactory : ITcpClientFactory
{
    public ITcpClientWrapper CreateClient()
    {
        return new TcpClientWrapper(new TcpClient());
    }
}

public class TcpClientWrapper : ITcpClientWrapper
{
    private readonly TcpClient _client;

    public TcpClientWrapper(TcpClient client)
    {
        _client = client;
    }

    public bool Connected => _client.Connected;

    public void Connect(string host, int port)
    {
        _client.Connect(host, port);
    }

    public Stream GetStream()
    {
        return _client.GetStream();
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}
