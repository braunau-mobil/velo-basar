#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Models.Entities;
//using BraunauMobil.VeloBasar.Routing;
//using BraunauMobil.VeloBasar.Validation;
//using BraunauMobil.VeloBasar.ViewModels;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.SellersControllerTests;

//public class Details
//{
//    [Theory]
//    [AutoVeloData]
//    public async Task ExistingSeller_ReturnsViewModel(BasarModel activeBasar, SellerEntity seller, IEnumerable<TransactionEntity> acceptances, IEnumerable<TransactionEntity> settlements, IEnumerable<ProductEntity> products)
//    {
//        var mockSellerContext = new Mock<ISellerContext>();
//        mockSellerContext.Setup(c => c.GetAsync(seller.Id))
//            .ReturnsAsync(seller);

//        var mockTransactionContext = new Mock<ITransactionContext>();
//        mockTransactionContext.Setup(c => c.GetManyAsync(activeBasar, TransactionType.Acceptance, seller.Id))
//            .ReturnsAsync(acceptances);
//        mockTransactionContext.Setup(c => c.GetManyAsync(activeBasar, TransactionType.Settlement, seller.Id))
//            .ReturnsAsync(settlements);

//        var mockProductContext = new Mock<IProductContext>();
//        mockProductContext.Setup(c => c.GetProductsForSellerAsync(activeBasar, seller))
//            .ReturnsAsync(products);

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockSellerContext.Object, Mockups.Txt, mockTransactionContext.Object, mockProductContext.Object, new Mock<ISellerRouter>().Object, new Mock<ISellerValidator>().Object);

//        var result = await controller.Details(seller.Id, activeBasar);

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<SellerViewModel>(viewResult.Model);

//        Assert.Equal(seller, vm.Seller);
//        Assert.False(vm.ShowNameAndId);
//        Assert.Equal(activeBasar, vm.Basar);
//        Assert.Equal(products, vm.Procucts);
//        Assert.Equal(acceptances, vm.Acceptances);
//        Assert.Equal(settlements, vm.Settlemens);

//        mockSellerContext.Verify(c => c.GetAsync(seller.Id), Times.Once());
//        mockTransactionContext.Verify(c => c.GetManyAsync(activeBasar, TransactionType.Acceptance, seller.Id), Times.Once());
//        mockTransactionContext.Verify(c => c.GetManyAsync(activeBasar, TransactionType.Settlement, seller.Id), Times.Once());
//        mockProductContext.Verify(c => c.GetProductsForSellerAsync(activeBasar, seller), Times.Once());
//    }

//    [Theory]
//    [AutoData]
//    public async Task NotExistingSeller_ReturnsNotFound(int sellerId, BasarModel activeBasar)
//    {
//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetAsync(sellerId));

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, new Mock<ISellerValidator>().Object);

//        var result = await controller.Details(sellerId, activeBasar);

//        Assert.IsType<NotFoundResult>(result);

//        mockContext.Verify(c => c.GetAsync(sellerId), Times.Once());
//    }
//}
