using RallyXAutoSplitter.Services;
using RallyXAutoSplitter.Tests.TestHelpers;
using Xunit;

namespace RallyXAutoSplitter.Tests;

public class AutoSplitterServiceTests
{
    private readonly FakeLogger _logger;
    private readonly LiveSplitServiceSpy _livesplit;
    private readonly AutoSplitterService _service;

    public AutoSplitterServiceTests()
    {
        _logger = new FakeLogger();
        _livesplit = new LiveSplitServiceSpy();
        _service = new AutoSplitterService(_livesplit, _logger);
    }

    [Fact]
    public void RaceStarted_Track0With3Laps_ShouldResetAndStart()
    {
        // Arrange
        var race = CreateRace(trackIndex: 0, lapCount: 3);

        // Act
        _service.RaceStarted(race);

        // Assert - Track 0 does Reset and Start (NO split)
        Assert.Equal(1, _livesplit.ResetRunCalls);
        Assert.Equal(1, _livesplit.StartTimerCalls);
        Assert.Equal(0, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceStarted_Track0WithNon3Laps_ShouldNotStartTimer()
    {
        // Arrange
        var race = CreateRace(trackIndex: 0, lapCount: 5);

        // Act
        _service.RaceStarted(race);

        // Assert
        Assert.Equal(0, _livesplit.StartTimerCalls);
        Assert.Equal(0, _livesplit.ResetRunCalls);
        Assert.Equal(0, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceStarted_Track1AfterTrack0_ShouldSplit()
    {
        // Arrange
        var race0 = CreateRace(trackIndex: 0, lapCount: 3);
        var race1 = CreateRace(trackIndex: 1, lapCount: 3);

        // Act
        _service.RaceStarted(race0); // Reset + Start (no split)
        _service.RaceStarted(race1); // Split (track 1)

        // Assert - Should have 1 split (track 1 start)
        Assert.Equal(1, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceStarted_Track2OutOfSequence_ShouldNotSplit()
    {
        // Arrange
        var race0 = CreateRace(trackIndex: 0, lapCount: 3);
        var race2 = CreateRace(trackIndex: 2, lapCount: 3); // Skip track 1

        // Act
        _service.RaceStarted(race0); // Start run (no split)
        _service.RaceStarted(race2); // Wrong order - should be ignored

        // Assert - No splits (track 0 doesn't split, track 2 is out of sequence)
        Assert.Equal(0, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceEnded_WinningPlayerNegativeOne_ShouldResetRun()
    {
        // Arrange
        var race = CreateRace(trackIndex: 0, lapCount: 3);

        // Act
        _service.RaceEnded(race, winningPlayer: -1);

        // Assert
        Assert.Equal(1, _livesplit.ResetRunCalls);
    }

    [Fact]
    public void RaceEnded_Track4WithFullSequence_ShouldSplitAndClearState()
    {
        // Arrange
        var race0 = CreateRace(trackIndex: 0, lapCount: 3);
        var race1 = CreateRace(trackIndex: 1, lapCount: 3);
        var race2 = CreateRace(trackIndex: 2, lapCount: 3);
        var race3 = CreateRace(trackIndex: 3, lapCount: 3);
        var race4 = CreateRace(trackIndex: 4, lapCount: 3);

        // Act - Complete a full run sequence
        _service.RaceStarted(race0); // Reset + Start (no split)
        _service.RaceStarted(race1); // Split
        _service.RaceStarted(race2); // Split
        _service.RaceStarted(race3); // Split
        _service.RaceStarted(race4); // Split
        _service.RaceEnded(race4, winningPlayer: 1); // Final split

        // Assert - 5 splits total (tracks 1,2,3,4 started + track 4 ended)
        Assert.Equal(5, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceEnded_WithoutActiveRun_ShouldNotSplit()
    {
        // Arrange
        var race = CreateRace(trackIndex: 4, lapCount: 3);

        // Act
        _service.RaceEnded(race, winningPlayer: 1);

        // Assert
        Assert.Equal(0, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceStarted_Track0Twice_ShouldNotStartSecondRun()
    {
        // Arrange
        var race0First = CreateRace(trackIndex: 0, lapCount: 3);
        var race0Second = CreateRace(trackIndex: 0, lapCount: 3);

        // Act
        _service.RaceStarted(race0First); // Start first run
        _service.RaceStarted(race0Second); // Should be ignored

        // Assert - Only one start, no splits
        Assert.Equal(1, _livesplit.StartTimerCalls);
        Assert.Equal(0, _livesplit.SplitCalls);
    }

    [Fact]
    public void RaceStarted_AllTracks_InSequence_ShouldSplitEach()
    {
        // Arrange
        var races = Enumerable.Range(0, 5)
            .Select(i => CreateRace((short)i, lapCount: 3))
            .ToArray();

        // Act
        foreach (var race in races)
        {
            _service.RaceStarted(race);
        }

        // Assert - 4 splits (tracks 1-4 start, track 0 doesn't split)
        Assert.Equal(4, _livesplit.SplitCalls);
    }

    private static FakeRaceData CreateRace(short trackIndex, short lapCount)
    {
        return new FakeRaceData(trackIndex, lapCount);
    }
}
