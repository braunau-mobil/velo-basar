using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Index
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CreatesNewCartModelAndReturnsView(IList<int> cart, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetCart())
            .Returns(cart);
        ProductService.Setup(_ => _.GetManyAsync(cart))
            .ReturnsAsync(products);

        //  Act
        IActionResult result = await Sut.Index();

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        CartModel model = viewResult.Model.Should().BeOfType<CartModel>().Subject;
        model.Products.Should().BeEquivalentTo(products);
        model.ActiveBasarId.Should().Be(0);
        model.ProductId.Should().Be(0);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        ProductService.Verify(_ => _.GetManyAsync(cart), Times.Once());
        VerifyNoOtherCalls();
    }
}
