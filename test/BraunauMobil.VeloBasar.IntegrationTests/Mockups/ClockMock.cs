using Xan.Extensions;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class ClockMock(DateTime now)
        : IClock
{
    public DateTimeOffset Now { get; set; } = now;

    public DateOnly GetCurrentDate()
        => DateOnly.FromDateTime(Now.DateTime);

    public DateTime GetCurrentDateTime()
        => Now.DateTime;

    public DateTimeOffset GetCurrentDateTimeOffset()
        => Now;

    public TimeOnly GetCurrentTime()
        => TimeOnly.FromDateTime(Now.DateTime);
}
