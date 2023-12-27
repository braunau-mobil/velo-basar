using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptanceLabelsControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new AcceptanceLabelsController(TransactionService, Localizer);
    }

    protected IStringLocalizer<SharedResources> Localizer { get; } = X.StrictFake<IStringLocalizer<SharedResources>>();

    protected Fixture Fixture { get; } = new ();

    protected ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();

    protected AcceptanceLabelsController Sut { get; }
}
