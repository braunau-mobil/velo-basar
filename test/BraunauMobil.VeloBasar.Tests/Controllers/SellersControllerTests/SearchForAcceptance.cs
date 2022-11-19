#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Models.Entities;
//using BraunauMobil.VeloBasar.Routing;
//using BraunauMobil.VeloBasar.Validation;
//using BraunauMobil.VeloBasar.ViewModels;
//using FluentValidation.Results;
//using Microsoft.AspNetCore.Mvc;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.SellersControllerTests;

//public class SearchForAcceptance
//{
//    [Theory]
//    [AutoData]
//    public async Task InvalidSeller_ReturnsViewModel(SellerEntity seller, IEnumerable<ValidationFailure> failures)
//    {
//        var mockValidator = new Mock<ISellerValidator>();
//        mockValidator.Setup(v => v.ValidateForSearch(seller))
//            .Returns(new ValidationResult(failures));

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetManyAsync(seller.FirstName, seller.LastName));

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, mockValidator.Object);

//        var result = await controller.SearchForAcceptance(seller);

//        Assert.False(controller.ModelState.IsValid);

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<CreateSellerViewModel>(viewResult.Model);

//        Assert.Empty(vm.FoundSellers);
//        Assert.True(vm.HasSearched);
//        Assert.Equal(seller, vm.Seller);

//        mockValidator.Verify(v => v.ValidateForSearch(seller), Times.Once());
//        mockContext.Verify(c => c.GetManyAsync(seller.FirstName, seller.LastName), Times.Never());
//    }

//    [Theory]
//    [AutoData]
//    public async Task ValidSeller_NothingFound_ReturnsViewModel(SellerEntity seller)
//    {
//        var mockValidator = new Mock<ISellerValidator>();
//        mockValidator.Setup(v => v.ValidateForSearch(seller))
//            .Returns(new ValidationResult());

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetManyAsync(seller.FirstName, seller.LastName))
//            .ReturnsAsync(Enumerable.Empty<SellerEntity>());

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, mockValidator.Object);

//        var result = await controller.SearchForAcceptance(seller);

//        Assert.True(controller.ModelState.IsValid);

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<CreateSellerViewModel>(viewResult.Model);

//        Assert.Empty(vm.FoundSellers);
//        Assert.True(vm.HasSearched);
//        Assert.Equal(seller, vm.Seller);

//        mockValidator.Verify(v => v.ValidateForSearch(seller), Times.Once());
//        mockContext.Verify(c => c.GetManyAsync(seller.FirstName, seller.LastName), Times.Once());
//    }

//    [Theory]
//    [AutoData]
//    public async Task ValidSeller_SomeFound_ReturnsViewModel(SellerEntity seller, IEnumerable<SellerEntity> sellers)
//    {
//        var mockValidator = new Mock<ISellerValidator>();
//        mockValidator.Setup(v => v.ValidateForSearch(seller))
//            .Returns(new ValidationResult());

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.GetManyAsync(seller.FirstName, seller.LastName))
//            .ReturnsAsync(sellers);

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, mockValidator.Object);

//        var result = await controller.SearchForAcceptance(seller);

//        Assert.True(controller.ModelState.IsValid);

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<CreateSellerViewModel>(viewResult.Model);

//        Assert.Equal(sellers, vm.FoundSellers.Select(item => item.Obj));
//        Assert.True(vm.HasSearched);
//        Assert.Equal(seller, vm.Seller);

//        mockValidator.Verify(v => v.ValidateForSearch(seller), Times.Once());
//        mockContext.Verify(c => c.GetManyAsync(seller.FirstName, seller.LastName), Times.Once());
//    }
//}
