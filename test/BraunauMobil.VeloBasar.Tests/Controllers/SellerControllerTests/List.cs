using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Mvc.Crud;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class List
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ExistingSellerId_ReturnsRedirectToDetails(SellerListParameter parameter, int sellerId, string url)
    {
        //  Arrange
        parameter.SearchString = sellerId.ToString();
        SellerService.Setup(_ => _.ExistsAsync(sellerId))
            .ReturnsAsync(true);
        SellerRouter.Setup(_ => _.ToDetails(sellerId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        SellerService.Verify(_ => _.ExistsAsync(sellerId), Times.Once());
        SellerRouter.Verify(_ => _.ToDetails(sellerId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task NotExistingSellerId_CallsGetManyAndReturnsView(SellerListParameter parameter, int sellerId)
    {
        //  Arrange
        IPaginatedList<CrudItemModel<SellerEntity>> list = Fixture.CreatePaginatedList<CrudItemModel<SellerEntity>>();
        parameter.SearchString = sellerId.ToString();
        SellerService.Setup(_ => _.ExistsAsync(sellerId))
            .ReturnsAsync(false);
        SellerService.Setup(_ => _.GetManyAsync(parameter))
            .ReturnsAsync(list);

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<CrudListModel<SellerEntity, SellerListParameter, ISellerRouter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        SellerService.Verify(_ => _.ExistsAsync(sellerId), Times.Once());
        SellerService.Verify(_ => _.GetManyAsync(parameter), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task CallsGetManyAndReturnsView(SellerListParameter parameter)
    {
        //  Arrange
        IPaginatedList<CrudItemModel<SellerEntity>> list = Fixture.CreatePaginatedList<CrudItemModel<SellerEntity>>();
        SellerService.Setup(_ => _.GetManyAsync(parameter))
            .ReturnsAsync(list);

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<CrudListModel<SellerEntity, SellerListParameter, ISellerRouter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        SellerService.Verify(_ => _.GetManyAsync(parameter), Times.Once());
        VerifyNoOtherCalls();
    }
}
