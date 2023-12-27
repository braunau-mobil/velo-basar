using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Tests;

public class VeloTextsTest
{
    private class MemoryLogger
        : ILogger
    {
        public List<string> LogEntries { get; } = new ();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => throw new NotImplementedException();

        public bool IsEnabled(LogLevel logLevel)
            => throw new NotImplementedException();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            LogEntries.Add($"{logLevel}_{eventId}_{formatter(state, exception)}");
        }
    }

    [Fact]
    public void AllShouldBeTranslated()
    {
        //  Arrange
        MemoryLogger logger = new ();

        //  Act
        VeloTexts.CheckIfAllIsTranslated(logger);

        //  Assert
        logger.LogEntries.Should().BeEmpty();
    }
}
