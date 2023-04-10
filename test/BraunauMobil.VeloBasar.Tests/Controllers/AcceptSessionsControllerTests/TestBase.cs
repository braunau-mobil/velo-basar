﻿using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionsControllerTests
{
    public class TestBase
    {
        public TestBase()
        {
            Sut = new AcceptSessionController(AcceptSessionService.Object, Router.Object, Localizer, Cookie.Object);

            Router.Setup(_ => _.AcceptSession)
                .Returns(AcceptSessionRouter.Object);
            Router.Setup(_ => _.Seller)
                .Returns(SellerRouter.Object);
        }

        public void VerifyNoOtherCalls()
        {
            AcceptSessionService.VerifyNoOtherCalls();
            AcceptSessionRouter.VerifyNoOtherCalls();
            Cookie.VerifyNoOtherCalls();
            Router.VerifyNoOtherCalls();
            SellerRouter.VerifyNoOtherCalls();
        }

        protected Mock<IAcceptSessionRouter> AcceptSessionRouter { get; } = new ();

        protected Mock<IAcceptSessionService> AcceptSessionService { get; } = new ();

        protected Mock<IActiveAcceptSessionCookie> Cookie { get; } = new ();

        protected Fixture Fixture { get; } = new ();

        protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();
        
        protected Mock<IVeloRouter> Router { get; } = new ();

        protected Mock<ISellerRouter> SellerRouter { get; } = new();

        protected AcceptSessionController Sut { get; }
    }
}
