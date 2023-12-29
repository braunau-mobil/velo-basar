using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests
{
    public class TestBase
    {
        public TestBase()
        {
            Sut = new AcceptSessionController(AcceptSessionService, Router, Localizer, Cookie);

            A.CallTo(() => Router.AcceptProduct).Returns(AcceptProductRouter);
            A.CallTo(() => Router.AcceptSession).Returns(AcceptSessionRouter);
            A.CallTo(() => Router.Seller).Returns(SellerRouter);
            A.CallTo(() => Router.Transaction).Returns(TransactionRouter);
        }

        protected IAcceptProductRouter AcceptProductRouter { get; } = X.StrictFake<IAcceptProductRouter>();
        
        protected IAcceptSessionRouter AcceptSessionRouter { get; } = X.StrictFake<IAcceptSessionRouter> ();

        protected IAcceptSessionService AcceptSessionService { get; } = X.StrictFake<IAcceptSessionService> ();

        protected IActiveAcceptSessionCookie Cookie { get; } = X.StrictFake<IActiveAcceptSessionCookie> ();

        protected Fixture Fixture { get; } = new ();

        protected IStringLocalizer<SharedResources> Localizer { get; } = new StringLocalizerMock<SharedResources>();
        
        protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter> ();

        protected ISellerRouter SellerRouter { get; } = X.StrictFake<ISellerRouter>();

        protected AcceptSessionController Sut { get; }

        protected ITransactionRouter TransactionRouter { get; } = X.StrictFake<ITransactionRouter> ();
    }
}
