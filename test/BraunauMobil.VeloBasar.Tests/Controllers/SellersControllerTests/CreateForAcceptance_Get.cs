#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Models.Entities;
//using BraunauMobil.VeloBasar.Routing;
//using BraunauMobil.VeloBasar.Validation;
//using BraunauMobil.VeloBasar.ViewModels;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.SellersControllerTests;

//public class CreateForAcceptance_Get
//{
//    [Theory]
//    [AutoData]
//    public async Task ReturnsNotFound(int sellerId)
//    {
//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetAsync(sellerId));

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, new Mock<ISellerValidator>().Object);

//        var result = await controller.CreateForAcceptance(sellerId);

//        Assert.IsType<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task NoSellerId_ReturnsEmptySeller()
//    {
//        var controller = new SellersController(new Mock<IVeloRouter>().Object, new Mock<ISellerContext>().Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, new Mock<ISellerValidator>().Object);

//        int? sellerId = null;
//        var result = await controller.CreateForAcceptance(sellerId);

//        var view = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<CreateSellerViewModel>(view.Model);
        
//        Assert.Empty(vm.FoundSellers);
//        Assert.False(vm.HasSearched);
//        Assert.Equal(0, vm.Seller.Id);
//    }

//    [Theory]
//    [AutoData]
//    public async Task ExistingSeller_ReturnsSeller(SellerEntity seller)
//    {
//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetAsync(seller.Id))
//            .ReturnsAsync(seller);

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, new Mock<ISellerValidator>().Object);

//        var result = await controller.CreateForAcceptance(seller.Id);

//        var view = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<CreateSellerViewModel>(view.Model);

//        Assert.Empty(vm.FoundSellers);
//        Assert.False(vm.HasSearched);
//        Assert.Equal(seller, vm.Seller);
//    }
//}
