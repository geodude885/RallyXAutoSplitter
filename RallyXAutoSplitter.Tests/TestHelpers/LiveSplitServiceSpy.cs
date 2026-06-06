using RallyXAutoSplitter.Services;

namespace RallyXAutoSplitter.Tests.TestHelpers;

public class LiveSplitServiceSpy : ILiveSplitService
{
    public int StartTimerCalls { get; private set; }
    public int SplitCalls { get; private set; }
    public int ResetRunCalls { get; private set; }

    public void StartTimer()
    {
        StartTimerCalls++;
    }

    public void Split()
    {
        SplitCalls++;
    }

    public void ResetRun()
    {
        ResetRunCalls++;
    }
}
