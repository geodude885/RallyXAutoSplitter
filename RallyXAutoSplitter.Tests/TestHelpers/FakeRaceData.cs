using RallyXAutoSplitter.Adapters;

namespace RallyXAutoSplitter.Tests.TestHelpers;

public class FakeRaceData : IRaceData
{
    public short SelectedTrackIndex { get; }
    public short SelectedLapCount { get; }

    public FakeRaceData(short trackIndex, short lapCount)
    {
        SelectedTrackIndex = trackIndex;
        SelectedLapCount = lapCount;
    }
}
