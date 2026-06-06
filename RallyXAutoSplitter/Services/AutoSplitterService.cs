using Il2CppRUMBLE.Environment.Minigames;
using RallyXAutoSplitter.Adapters;

namespace RallyXAutoSplitter.Services;

public sealed class AutoSplitterService
{
    private readonly ILogger _logger;
    private readonly ILiveSplitService _livesplit;

    private bool _runActive;
    private short _expectedTrack = 0;

    public AutoSplitterService(
        ILiveSplitService livesplit,
        ILogger logger)
    {
        _logger = logger;
        _livesplit = livesplit;
    }

    public void RaceStarted(RockRace race)
    {
        var raceData = new RockRaceAdapter(race);
        RaceStarted(raceData);
    }

    public void RaceStarted(IRaceData race)
    {
        short track = race.SelectedTrackIndex;
        short laps = race.SelectedLapCount;

        if (laps != 3)
            return;

        switch (track)
        {
            case 0:
                if (_expectedTrack != 0)
                    return;

                _runActive = true;
                _expectedTrack = 1;

                // Start a fresh run (no split for track 0)
                _livesplit.ResetRun();
                _livesplit.StartTimer();

                _logger.Msg("RaceStarted: Track 0 - Timer started");
                break;

            case 1:
            case 2:
            case 3:
            case 4:
                if (!_runActive)
                    return;

                if (track != _expectedTrack)
                {
                    _logger.Warning(
                        $"Ignoring unexpected track {track}, expected {_expectedTrack}");
                    return;
                }

                _expectedTrack++;

                // Split at the START of tracks 1-4
                _livesplit.Split();

                _logger.Msg($"RaceStarted: Track {track} - Split");
                break;
        }
    }

    public void RaceEnded(RockRace race, short winningPlayer)
    {
        var raceData = new RockRaceAdapter(race);
        RaceEnded(raceData, winningPlayer);
    }

    public void RaceEnded(IRaceData race, short winningPlayer)
    {
        // Park reset / race cancelled
        if (winningPlayer == -1)
        {
            _livesplit.ResetRun();
            ClearRunState();

            _logger.Msg("RaceEnded: Detected reset.");
            return;
        }

        if (!_runActive)
            return;

        // Final race completion (track 4 end)
        if (race.SelectedTrackIndex == 4 &&
            race.SelectedLapCount == 3)
        {
            if (_expectedTrack == 5)
            {
                _livesplit.Split();

                _logger.Msg("RaceEnded: Track 4 completed - Final split.");
            }
            else
            {
                _logger.Warning(
                    $"Track 4 completed but expected track state was {_expectedTrack}");
            }

            ClearRunState();
        }
    }

    private void ClearRunState()
    {
        _runActive = false;
        _expectedTrack = 0;

        _logger.Msg("State cleared.");
    }
}