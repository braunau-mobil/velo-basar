#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Routing;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarsControllerTests;

//public class ActiveBasarDetails
//{
//    [Theory]
//    [AutoData]
//    public void ReturnsRedirectToBasarDetails(BasarModel activeBasar)
//    {
//        var mockRouter = new Mock<IBasarRouter>();
//        mockRouter.Setup(router => router.ToDetails(activeBasar))
//            .Returns(activeBasar.Name);

//        var controller = new BasarsController(mockRouter.Object, new Mock<IBasarService>().Object, new Mock<IStatisticContext>().Object, Mockups.Txt);

//        var result = controller.ActiveBasarDetails(activeBasar);

//        var redirectResult = Assert.IsType<RedirectResult>(result);
//        Assert.Equal(activeBasar.Name, redirectResult.Url);
//    }
//}
