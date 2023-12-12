using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public class MockClock
    : IClock
{
    public DateTimeOffset Now { get; set; }

    public DateOnly GetCurrentDate()
        => DateOnly.FromDateTime(Now.DateTime);

    public DateTime GetCurrentDateTime()
        => Now.DateTime;

    public DateTimeOffset GetCurrentDateTimeOffset()
        => Now;

    public TimeOnly GetCurrentTime()
        => TimeOnly.FromDateTime(Now.DateTime);
}
