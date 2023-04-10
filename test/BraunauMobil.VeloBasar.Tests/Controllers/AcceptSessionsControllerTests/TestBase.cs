using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionsControllerTests
{
    public class TestBase
    {
        private readonly Mock<HttpResponse> _response = new();

        public TestBase()
        {
            _response.Setup(_ => _.Cookies)
                .Returns(ResponseCookies.Object);
            HttpContext.Setup(_ => _.Response)
                .Returns(_response.Object);

            Sut = new AcceptSessionController(AcceptSessionService.Object, Router.Object, Localizer)
            {
                ControllerContext = new()
                {
                    HttpContext = HttpContext.Object
                }
            };

            Router.Setup(_ => _.AcceptSession)
                .Returns(AcceptSessionRouter.Object);
            Router.Setup(_ => _.Seller)
                .Returns(SellerRouter.Object);
        }

        public void VerifyNoOtherCalls()
        {
            AcceptSessionService.VerifyNoOtherCalls();
            AcceptSessionRouter.VerifyNoOtherCalls();
            ResponseCookies.VerifyNoOtherCalls();
            Router.VerifyNoOtherCalls();
            SellerRouter.VerifyNoOtherCalls();
        }

        protected Mock<IAcceptSessionRouter> AcceptSessionRouter { get; } = new ();

        protected Mock<IAcceptSessionService> AcceptSessionService { get; } = new ();

        protected Fixture Fixture { get; } = new ();

        protected Mock<HttpContext> HttpContext { get; } = new();

        protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();
        
        protected Mock<IResponseCookies> ResponseCookies { get; } = new();

        protected Mock<IVeloRouter> Router { get; } = new ();

        protected Mock<ISellerRouter> SellerRouter { get; } = new();

        protected AcceptSessionController Sut { get; }
    }
}
