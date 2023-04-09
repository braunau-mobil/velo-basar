using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests
{
    public class TestBase
    {
        public TestBase()
        {
            Validator = new (Localizer);
            Sut = new AcceptProductController(AcceptProductService.Object, Router.Object, Validator);

            Router.Setup(_ => _.AcceptProduct)
                .Returns(AcceptProductRouter.Object);
        }

        public void VerifyNoOtherCalls()
        {
            AcceptProductService.VerifyNoOtherCalls();
            AcceptProductRouter.VerifyNoOtherCalls();
            Router.VerifyNoOtherCalls();
        }

        protected Mock<IAcceptProductRouter> AcceptProductRouter { get; } = new ();

        protected Mock<IAcceptProductService> AcceptProductService { get; } = new ();

        protected Fixture Fixture { get; } = new ();

        protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();
        
        protected Mock<IVeloRouter> Router { get; } = new ();
        
        protected AcceptProductController Sut { get; }

        protected ProductEntityValidator Validator { get; }
    }
}
