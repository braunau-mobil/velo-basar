using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests;

public class Mockups
{
    public static Mock<IClock> Clock(DateTime now)
    {
        Mock<IClock> mockClock = new ();
        mockClock.Setup(m => m.GetCurrentDateTime())
            .Returns(now);
        return mockClock;
    }
}
