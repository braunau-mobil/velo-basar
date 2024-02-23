using Xan.Extensions;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class ClockMock(Func<DateTime> getDateTime)
        : IClock
{
    public DateTimeOffset Now { get => getDateTime(); }

    public DateOnly GetCurrentDate()
        => DateOnly.FromDateTime(Now.DateTime);

    public DateTime GetCurrentDateTime()
        => Now.DateTime;

    public DateTimeOffset GetCurrentDateTimeOffset()
        => Now;

    public TimeOnly GetCurrentTime()
        => TimeOnly.FromDateTime(Now.DateTime);
}
