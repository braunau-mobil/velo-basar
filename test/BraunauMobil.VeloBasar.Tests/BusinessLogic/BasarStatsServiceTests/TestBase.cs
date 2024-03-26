using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new BasarStatsService(ColorProvider, Db, X.FormatProvider, Options.Create(ApplicationSettings));
    }

    public ApplicationSettings ApplicationSettings { get; } = new();

    public IColorProvider ColorProvider { get; } = X.StrictFake<IColorProvider>();

    public BasarStatsService Sut { get; }
}
