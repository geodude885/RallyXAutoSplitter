using Il2CppRUMBLE.Environment.Minigames;

namespace RallyXAutoSplitter.Adapters;

public interface IRaceData
{
    short SelectedTrackIndex { get; }
    short SelectedLapCount { get; }
}

public class RockRaceAdapter : IRaceData
{
    private readonly RockRace _race;

    public RockRaceAdapter(RockRace race)
    {
        _race = race;
    }

    public short SelectedTrackIndex => _race.selectedTrackIndex;
    public short SelectedLapCount => _race.selectedLapCount;
}
