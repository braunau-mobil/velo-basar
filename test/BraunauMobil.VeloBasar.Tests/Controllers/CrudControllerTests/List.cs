#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Routing;
//using BraunauMobil.VeloBasar.Validation;
//using BraunauMobil.VeloBasar.ViewModels;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.CrudControllerTests;

//public class List
//{
//    [Theory]
//    [AutoData]
//    public async Task NoSellers_ReturnsViewModel(string searchString, ValueState? valueState, int pageSize, int pageIndex)
//    {
//        var router = Mockups.Router(m =>
//        {
//            m.Setup(r => r.Sellers)
//                .Returns(new Mock<ISellersRouter>().Object);
//        });

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetManyAsync(searchString, valueState, pageSize, pageIndex))
//            .ReturnsAsync(Enumerable.Empty<Seller>().AsPaginated(pageSize, pageIndex));

//        var controller = new SellersController(router, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellersRouter>().Object, new Mock<ISellerValidator>().Object);

//        var result = await controller.List(new Parameters.SellersParameter { SearchString = searchString, ValueState = valueState, PageIndex = pageIndex, PageSize = pageSize });

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<FilterSellersViewModel>(viewResult.Model);

//        Assert.Empty(vm);
//        Assert.Equal(valueState, vm.ValueStateFilter);

//        mockContext.Verify(c => c.GetManyAsync(searchString, valueState, pageSize, pageIndex), Times.Once());
//    }

//    [Theory]
//    [AutoData]
//    public async Task Sellers_ReturnsViewModel(string searchString, ValueState? valueState, IEnumerable<Seller> sellers)
//    {
//        int pageIndex = 1;
//        int pageSize = 0;

//        var router = Mockups.Router(m =>
//        {
//            m.Setup(r => r.Sellers)
//                .Returns(new Mock<ISellersRouter>().Object);
//        });

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetManyAsync(searchString, valueState, pageSize, pageIndex))
//            .ReturnsAsync(sellers.AsPaginated(pageSize, pageIndex));

//        var controller = new SellersController(router, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellersRouter>().Object, new Mock<ISellerValidator>().Object);

//        var result = await controller.List(new Parameters.SellersParameter { SearchString = searchString, ValueState = valueState, PageIndex = pageIndex, PageSize = pageSize });

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<FilterSellersViewModel>(viewResult.Model);

//        Assert.Equal(sellers, vm);
//        Assert.Equal(valueState, vm.ValueStateFilter);

//        mockContext.Verify(c => c.GetManyAsync(searchString, valueState, pageSize, pageIndex), Times.Once());
//    }
//}
