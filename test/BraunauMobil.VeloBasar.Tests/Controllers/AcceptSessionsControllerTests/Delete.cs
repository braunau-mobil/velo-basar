#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Routing;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionsControllerTests;

//public class Delete
//{
//    [Theory]
//    [AutoData]
//    public async void DeletesSession_ReturnsRedirectToSellerDetails(int sessionId, int sellerId)
//    {
//        var seller = new Seller { Id = sellerId };
//        var acceptSession = new AcceptSessionMODEL { Id = sessionId, Seller = seller, SellerId = sellerId };

//        var mockSellerRouter = new Mock<ISellerRouter>();
//        mockSellerRouter.Setup(router => router.ToDetails(seller))
//            .Returns(seller.Id.ToString());

//        var mockRouter = new Mock<IVeloRouter>();
//        mockRouter.Setup(router => router.Seller)
//            .Returns(mockSellerRouter.Object);
//        var mockSessionContext = new Mock<IAcceptSessionContext>();
//        mockSessionContext.Setup(context => context.DeleteAsync(acceptSession))
//            .Callback(() => acceptSession.Id = 0);
//        mockSessionContext.Setup(context => context.GetAsync(sessionId))
//            .ReturnsAsync(acceptSession);

//        var controller = new AcceptSessionsController(mockRouter.Object, mockSessionContext.Object);

//        var result = await controller.Delete(sessionId);

//        var redirectResult = Assert.IsType<RedirectResult>(result);
//        Assert.Equal(seller.Id.ToString(), redirectResult.Url);

//        Assert.Equal(0, acceptSession.Id);
//    }

//    [Theory]
//    [AutoData]
//    public async void ReturnsNotFound(int sessionId)
//    {
//        var mockRouter = new Mock<IVeloRouter>();
//        var mockSessionContext = new Mock<IAcceptSessionContext>();

//        var controller = new AcceptSessionsController(mockRouter.Object, mockSessionContext.Object);

//        var result = await controller.Delete(sessionId);

//        Assert.IsType<NotFoundResult>(result);
//    }
//}
