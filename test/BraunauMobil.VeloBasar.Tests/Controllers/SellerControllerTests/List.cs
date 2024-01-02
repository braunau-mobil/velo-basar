using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Crud;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class List
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ExistingSellerId_ReturnsRedirectToDetails(SellerListParameter parameter, int sellerId, string url)
    {
        //  Arrange
        parameter.SearchString = sellerId.ToString();
        A.CallTo(() => SellerService.ExistsAsync(sellerId)).Returns(true);
        A.CallTo(() => SellerRouter.ToDetails(sellerId)).Returns(url);

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => SellerService.ExistsAsync(sellerId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => SellerRouter.ToDetails(sellerId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NotExistingSellerId_CallsGetManyAndReturnsView(SellerListParameter parameter, int sellerId)
    {
        //  Arrange
        IPaginatedList<CrudItemModel<SellerEntity>> list = Fixture.CreatePaginatedList<CrudItemModel<SellerEntity>>();
        parameter.SearchString = sellerId.ToString();
        A.CallTo(() => SellerService.ExistsAsync(sellerId)).Returns(false);
        A.CallTo(() => SellerService.GetManyAsync(parameter)).Returns(list);

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<CrudListModel<SellerEntity, SellerListParameter, ISellerRouter>>();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => SellerService.ExistsAsync(sellerId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => SellerService.GetManyAsync(parameter)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task CallsGetManyAndReturnsView(SellerListParameter parameter)
    {
        //  Arrange
        IPaginatedList<CrudItemModel<SellerEntity>> list = Fixture.CreatePaginatedList<CrudItemModel<SellerEntity>>();
        A.CallTo(() => SellerService.GetManyAsync(parameter)).Returns(list);

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<CrudListModel<SellerEntity, SellerListParameter, ISellerRouter>>();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => SellerService.GetManyAsync(parameter)).MustHaveHappenedOnceExactly();
    }
}
