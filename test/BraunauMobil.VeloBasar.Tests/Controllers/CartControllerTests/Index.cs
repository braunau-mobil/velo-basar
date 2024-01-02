using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Index
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task CreatesNewCartModelAndReturnsView(IList<int> cart, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        A.CallTo(() => Cookie.GetCart()).Returns(cart);
        A.CallTo(() => ProductService.GetManyAsync(cart)).Returns(products);

        //  Act
        IActionResult result = await Sut.Index();

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            CartModel model = view.Model.Should().BeOfType<CartModel>().Subject;
            model.Products.Should().BeEquivalentTo(products);
            model.ActiveBasarId.Should().Be(0);
            model.ProductId.Should().Be(0);
        }

        A.CallTo(() => Cookie.GetCart()).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductService.GetManyAsync(cart)).MustHaveHappenedOnceExactly();
    }
}
