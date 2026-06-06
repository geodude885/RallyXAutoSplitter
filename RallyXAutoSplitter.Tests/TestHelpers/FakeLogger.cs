using RallyXAutoSplitter.Adapters;

namespace RallyXAutoSplitter.Tests.TestHelpers;

public class FakeLogger : ILogger
{
    public List<string> Messages { get; } = new();
    public List<string> Warnings { get; } = new();
    public List<string> Errors { get; } = new();

    public void Msg(string message)
    {
        Messages.Add(message);
    }

    public void Warning(string message)
    {
        Warnings.Add(message);
    }

    public void Error(string message)
    {
        Errors.Add(message);
    }
}
