using RallyXAutoSplitter.Services;
using RallyXAutoSplitter.Tests.TestHelpers;
using Xunit;

namespace RallyXAutoSplitter.Tests;

public class LiveSplitServiceTests
{
    [Fact]
    public void StartTimer_ShouldLogCorrectMessage()
    {
        // Arrange
        var logger = new FakeLogger();
        var tcpFactory = new FakeTcpClientFactory { ShouldConnect = true };
        var service = new LiveSplitService(logger, tcpFactory);

        // Act
        service.StartTimer();

        // Assert
        Assert.Contains("[AUTOSPLITTER] START TIMER", logger.Messages);
    }

    [Fact]
    public void Split_ShouldLogCorrectMessage()
    {
        // Arrange
        var logger = new FakeLogger();
        var tcpFactory = new FakeTcpClientFactory { ShouldConnect = true };
        var service = new LiveSplitService(logger, tcpFactory);

        // Act
        service.Split();

        // Assert
        Assert.Contains("[AUTOSPLITTER] SPLIT", logger.Messages);
    }

    [Fact]
    public void ResetRun_ShouldLogCorrectMessage()
    {
        // Arrange
        var logger = new FakeLogger();
        var tcpFactory = new FakeTcpClientFactory { ShouldConnect = true };
        var service = new LiveSplitService(logger, tcpFactory);

        // Act
        service.ResetRun();

        // Assert
        Assert.Contains("[AUTOSPLITTER] RESET", logger.Messages);
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var logger = new FakeLogger();
        var tcpFactory = new FakeTcpClientFactory { ShouldConnect = true };
        var service = new LiveSplitService(logger, tcpFactory);

        // Act & Assert
        var exception = Record.Exception(() => service.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void EnsureConnected_WithConnectionFailure_ShouldReturnFalse()
    {
        // Arrange
        var logger = new FakeLogger();
        var tcpFactory = new FakeTcpClientFactory { ShouldConnect = false };
        var service = new LiveSplitService(logger, tcpFactory);

        // Act
        var result = service.EnsureConnected();

        // Assert
        Assert.False(result);
        Assert.Contains("Could not connect to LiveSplit", logger.Warnings.FirstOrDefault() ?? "");
    }

    [Fact]
    public void EnsureConnected_WithSuccessfulConnection_ShouldReturnTrue()
    {
        // Arrange
        var logger = new FakeLogger();
        var tcpFactory = new FakeTcpClientFactory { ShouldConnect = true };
        var service = new LiveSplitService(logger, tcpFactory);

        // Act
        var result = service.EnsureConnected();

        // Assert
        Assert.True(result);
    }
}
