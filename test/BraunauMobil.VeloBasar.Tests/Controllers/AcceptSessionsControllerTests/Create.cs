#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Routing;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionsControllerTests;

//public class Create
//{
//    [Theory]
//    [AutoData]
//    public async void CreatesSession_ReturnsRedirectToCreateSessionProduct(int sellerId, BasarModel activeBasar, int sessionId)
//    {
//        var acceptSession = new AcceptSessionMODEL { Id = sessionId };

//        var mockSessionProductsRouter = new Mock<ISessionProductRouter>();
//        mockSessionProductsRouter.Setup(router => router.ToCreate(acceptSession))
//            .Returns(acceptSession.Id.ToString());
//        var mockRouter = new Mock<IVeloRouter>();
//        mockRouter.Setup(router => router.SessionProducts)
//            .Returns(mockSessionProductsRouter.Object);
//        var mockSessionContext = new Mock<IAcceptSessionContext>();
//        mockSessionContext.Setup(context => context.CreateAsync(activeBasar.Id, sellerId))
//            .ReturnsAsync(acceptSession);

//        var controller = new AcceptSessionsController(mockRouter.Object, mockSessionContext.Object);

//        var result = await controller.Create(sellerId, activeBasar);

//        var redirectResult = Assert.IsType<RedirectResult>(result);
//        Assert.Equal(acceptSession.Id.ToString(), redirectResult.Url);
//    }
//}
