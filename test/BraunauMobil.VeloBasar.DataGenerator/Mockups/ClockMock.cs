using Xan.Extensions;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class ClockMock
    : IClock
{
    public DateOnly GetCurrentDate()
        => new(2063, 04, 05);

    public DateTime GetCurrentDateTime()
        => new(2063, 04, 05, 11, 22, 33, 666);

    public DateTimeOffset GetCurrentDateTimeOffset()
        => new(2063, 04, 05, 11, 22, 33, 666, TimeSpan.Zero);

    public TimeOnly GetCurrentTime()
        => new(11, 22, 33, 666);
}
