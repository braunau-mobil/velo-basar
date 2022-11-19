#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Models.Interfaces;
//using BraunauMobil.VeloBasar.Parameters;
//using BraunauMobil.VeloBasar.Routing.Base;
//using Microsoft.AspNetCore.Mvc;
//using Xan.AspNetCore.Mvc.Crud;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.CrudControllerTests;

//public abstract class AbstractCreate<TModel, TListParameter>
//    where TModel : class, IModel, new()
//    where TListParameter : ListParameter
//{
//    protected abstract CrudControllerOLD<BasarModel, ListParameter> CreateSut(ICrudContext<TModel> context, ICrudRouterOLD<TModel> router);

//    [Fact]
//    public void ReturnsViewResult()
//    {
//        var mockContext = new Mock<ICrudContext<TModel>>();
//        var mockRouter = new Mock<ICrudRouterOLD<TModel>>();

//        var sut = CreateSut(mockContext.Object, mockRouter.Object);

//        var result = sut.Create();
//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsType<TModel>(viewResult.Model);
//        Assert.NotNull(model);

//        mockContext.VerifyNoOtherCalls();
//        mockRouter.VerifyNoOtherCalls();
//    }
//}

