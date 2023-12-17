using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Add
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task InvalidCartModel_IsNotAddedToCookieAndReturnsView(CartModel cartModel, IList<int> cart, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetCart())
            .Returns(cart);
        ProductService.Setup(_ => _.GetManyAsync(cart))
            .ReturnsAsync(products);

        //  Act
        IActionResult result = await Sut.Add(cartModel);

        // Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        view.Model.Should().NotBeNull();
        view.Model.Should().Be(cartModel);

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        ProductService.Verify(_ => _.GetManyAsync(cart), Times.Once());
        ProductService.Verify(_ => _.FindAsync(cartModel.ProductId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ValidCartModel_IsAddedToCookieAndReturnsView(CartModel cartModel, ProductEntity product, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        int productId = cartModel.ProductId;
        Mock<IList<int>> cartMock = new();
        Cookie.Setup(_ => _.GetCart())
            .Returns(cartMock.Object);
        ProductService.Setup(_ => _.GetManyAsync(cartMock.Object))
            .ReturnsAsync(products);
        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;
        ProductService.Setup(_ => _.FindAsync(cartModel.ProductId))
            .ReturnsAsync(product);

        //  Act
        IActionResult result = await Sut.Add(cartModel);

        // Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        view.Model.Should().NotBeNull();
        view.Model.Should().Be(cartModel);

        cartModel.ProductId.Should().Be(0);
        cartMock.Verify(_ => _.Add(productId));
        cartMock.VerifyNoOtherCalls();
        Cookie.Verify(_ => _.GetCart(), Times.Once());
        Cookie.Verify(_ => _.SetCart(It.IsAny<IList<int>>()), Times.Once());
        ProductService.Verify(_ => _.GetManyAsync(cartMock.Object), Times.Once());
        ProductService.Verify(_ => _.FindAsync(productId), Times.Once());
        VerifyNoOtherCalls();
    }
}
