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
        A.CallTo(() => ProductService.ExistsForBasarAsync(activeBasarId, productId)).Returns(true);
        A.CallTo(() => ProductRouter.ToDetails(productId)).Returns(url);

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => ProductService.ExistsForBasarAsync(activeBasarId, productId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductRouter.ToDetails(productId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task NotExistingProductId_CallsGetManyAndReturnsView(ProductListParameter parameter, int productId, int activeBasarId)
    {
        //  Arrange
        IPaginatedList<ProductEntity> list = Fixture.CreatePaginatedList<ProductEntity>();
        parameter.SearchString = productId.ToString();
        A.CallTo(() => ProductService.ExistsForBasarAsync(activeBasarId, productId)).Returns(false);
        A.CallTo(() => ProductService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId)).Returns(list);

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<ListModel<ProductEntity, ProductListParameter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        A.CallTo(() => ProductService.ExistsForBasarAsync(activeBasarId, productId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task CallsGetManyAndReturnsView(ProductListParameter parameter, int activeBasarId)
    {
        //  Arrange
        IPaginatedList<ProductEntity> list = Fixture.CreatePaginatedList<ProductEntity>();
        A.CallTo(() => ProductService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId)).Returns(list);

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<ListModel<ProductEntity, ProductListParameter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        A.CallTo(() => ProductService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId)).MustHaveHappenedOnceExactly();
    }
}
