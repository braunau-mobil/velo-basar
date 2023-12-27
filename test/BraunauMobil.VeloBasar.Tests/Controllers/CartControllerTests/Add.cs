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
        A.CallTo(() => Cookie.GetCart()).Returns(cart);
        A.CallTo(() => ProductService.GetManyAsync(cart)).Returns(products);
        A.CallTo(() => ProductService.FindAsync(cartModel.ProductId)).Returns((ProductEntity?)null);

        //  Act
        IActionResult result = await Sut.Add(cartModel);

        // Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();

            view.Model.Should().NotBeNull();
            view.Model.Should().Be(cartModel);
        }

        A.CallTo(() => Cookie.GetCart()).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductService.GetManyAsync(cart)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductService.FindAsync(cartModel.ProductId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task ValidCartModel_IsAddedToCookieAndReturnsView(CartModel cartModel, ProductEntity product, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        int productId = cartModel.ProductId;
        List<int> cart = new ();
        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;
        A.CallTo(() => ProductService.FindAsync(cartModel.ProductId)).Returns(product);
        A.CallTo(() => Cookie.SetCart(cart)).DoesNothing();
        A.CallTo(() => Cookie.GetCart()).Returns(cart);
        A.CallTo(() => ProductService.GetManyAsync(cart)).Returns(products);

        //  Act
        IActionResult result = await Sut.Add(cartModel);

        // Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();

            view.Model.Should().NotBeNull();
            view.Model.Should().Be(cartModel);
            cartModel.ProductId.Should().Be(0);
            cart.Should().ContainSingle(x => x == productId);
        }

        A.CallTo(() => Cookie.GetCart()).MustHaveHappenedOnceExactly();
        A.CallTo(() => Cookie.SetCart(cart)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductService.GetManyAsync(cart)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductService.FindAsync(productId)).MustHaveHappenedOnceExactly();
    }
}
