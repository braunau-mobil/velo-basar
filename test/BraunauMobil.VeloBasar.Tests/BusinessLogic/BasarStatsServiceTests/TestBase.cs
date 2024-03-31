using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        SellerService sellerService = new(X.StrictFake<ITransactionService>(), X.StrictFake<IDocumentService>(), X.StrictFake<IStatusPushService>(), X.StrictFake<ITokenProvider>(), Clock, Db, X.StringLocalizer);
        Sut = new BasarStatsService(ColorProvider, Db, X.FormatProvider, Options.Create(ApplicationSettings), sellerService);
    }

    public ApplicationSettings ApplicationSettings { get; } = new();

    public IColorProvider ColorProvider { get; } = X.StrictFake<IColorProvider>();

    public BasarStatsService Sut { get; }
}
