using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class ClockMock
    : IClock
{
    public ClockMock()
    { }

    public ClockMock(DateTime now)
    {
        Now = new DateTimeOffset(now);
    }

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
