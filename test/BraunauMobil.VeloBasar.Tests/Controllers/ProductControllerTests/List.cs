using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class List
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ExistingProductId_ReturnsRedirectToDetails(ProductListParameter parameter, int productId, int activeBasarId, string url)
    {
        //  Arrange
        parameter.SearchString = productId.ToString();
        ProductService.Setup(_ => _.ExistsForBasarAsync(activeBasarId, productId))
            .ReturnsAsync(true);
        ProductRouter.Setup(_ => _.ToDetails(productId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        ProductService.Verify(_ => _.ExistsForBasarAsync(activeBasarId, productId), Times.Once());
        ProductRouter.Verify(_ => _.ToDetails(productId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task NotExistingProductId_CallsGetManyAndReturnsView(ProductListParameter parameter, int productId, int activeBasarId)
    {
        //  Arrange
        IPaginatedList<ProductEntity> list = Fixture.CreatePaginatedList<ProductEntity>();
        parameter.SearchString = productId.ToString();
        ProductService.Setup(_ => _.ExistsForBasarAsync(activeBasarId, productId))
            .ReturnsAsync(false);
        ProductService.Setup(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId))
            .ReturnsAsync(list);

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<ListModel<ProductEntity, ProductListParameter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        ProductService.Verify(_ => _.ExistsForBasarAsync(activeBasarId, productId), Times.Once());
        ProductService.Verify(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task CallsGetManyAndReturnsView(ProductListParameter parameter, int activeBasarId)
    {
        //  Arrange
        IPaginatedList<ProductEntity> list = Fixture.CreatePaginatedList<ProductEntity>();
        ProductService.Setup(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId))
            .ReturnsAsync(list);

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<ListModel<ProductEntity, ProductListParameter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        ProductService.Verify(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId), Times.Once());
        VerifyNoOtherCalls();
    }
}
