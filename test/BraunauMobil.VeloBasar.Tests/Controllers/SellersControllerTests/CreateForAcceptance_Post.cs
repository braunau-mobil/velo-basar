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

//public class CreateForAcceptance_Post
//{
//    [Theory]
//    [AutoData]
//    public async Task InvalidSeller_ReturnsViewModel(SellerEntity seller)
//    {
//        var mockValidator = Mockups.ValidatorFalse<ISellerValidator, SellerEntity>(seller);

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.CreateAsync(seller));
//        mockContext.Setup(c => c.UpdateAsync(seller));

//        var controller = new SellersController(new Mock<IVeloRouter>().Object, new Mock<ISellerContext>().Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, mockValidator.Object);

//        var result = await controller.CreateForAcceptance(seller);

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var vm = Assert.IsType<CreateSellerViewModel>(viewResult.Model);

//        mockContext.Verify(c => c.CreateAsync(seller), Times.Never());
//        mockContext.Verify(c => c.UpdateAsync(seller), Times.Never());

//        Assert.Empty(vm.FoundSellers);
//        Assert.False(vm.HasSearched);
//        Assert.Equal(seller, vm.Seller);
//    }

//    [Theory]
//    [AutoData]
//    public async Task ValidSeller_Create_ReturnsRedirectToCreateAcceptSession(SellerEntity seller, string url)
//    {
//        seller.Id = 0;

//        var mockValidator = Mockups.ValidatorTrue<ISellerValidator, SellerEntity>(seller);

//        var mockAcceptSesionsRouter = new Mock<IAcceptRouter>();
//        mockAcceptSesionsRouter.Setup(router => router.ToCreate(seller))
//            .Returns(url);

//        var router = Mockups.Router(m =>
//        {
//            m.Setup(r => r.AcceptSession)
//                .Returns(mockAcceptSesionsRouter.Object);
//        });

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.CreateAsync(seller));

//        var controller = new SellersController(router, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, mockValidator.Object);

//        var result = await controller.CreateForAcceptance(seller);

//        mockContext.Verify(c => c.CreateAsync(seller), Times.Once());
//        mockAcceptSesionsRouter.Verify(r => r.ToCreate(seller), Times.Once());
//        mockValidator.Verify(v => v.Validate(seller), Times.Once());

//        var redirectResult = Assert.IsType<RedirectResult>(result);
//        Assert.Equal(url, redirectResult.Url);
//    }

//    [Theory]
//    [AutoData]
//    public async Task ValidSeller_Update_ReturnsRedirectToCreateAcceptSession(SellerEntity seller, string url)
//    {
//        var mockValidator = new Mock<ISellerValidator>();
//        mockValidator.Setup(v => v.Validate(seller))
//            .Returns(new ValidationResult());

//        var mockAcceptSesionsRouter = new Mock<IAcceptRouter>();
//        mockAcceptSesionsRouter.Setup(r => r.ToCreate(seller))
//            .Returns(url);
//        var router = Mockups.Router(m =>
//        {
//            m.Setup(r => r.AcceptSession)
//                .Returns(mockAcceptSesionsRouter.Object);
//        });

//        var mockContext = new Mock<ISellerContext>();
//        mockContext.Setup(c => c.UpdateAsync(seller));

//        var controller = new SellersController(router, mockContext.Object, Mockups.Txt, new Mock<ITransactionContext>().Object, new Mock<IProductContext>().Object, new Mock<ISellerRouter>().Object, mockValidator.Object);

//        var result = await controller.CreateForAcceptance(seller);

//        mockContext.Verify(c => c.UpdateAsync(seller), Times.Once());
//        mockAcceptSesionsRouter.Verify(r => r.ToCreate(seller), Times.Once());
//        mockValidator.Verify(v => v.Validate(seller), Times.Once());

//        var redirectResult = Assert.IsType<RedirectResult>(result);
//        Assert.Equal(url, redirectResult.Url);
//    }
//}
