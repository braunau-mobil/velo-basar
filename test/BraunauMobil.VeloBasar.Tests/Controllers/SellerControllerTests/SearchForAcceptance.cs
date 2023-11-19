using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class SearchForAcceptance
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task InvalidModel_ReturnsViewWithEmptyList(SellerSearchModel searchModel)
    {
        //  Arrange
        searchModel.FirstName = "";

        //  Act
        IActionResult result = await Sut.SearchForAcceptance(searchModel);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewName.Should().Be(nameof(SellerController.CreateForAcceptance));
        view.ViewData.ModelState.IsValid.Should().BeFalse();
        
        SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
        model.FoundSellers.Should().BeEmpty();
        model.HasSearched.Should().BeTrue();
        model.Seller.Should().NotBeNull();
        model.Seller.FirstName.Should().Be(searchModel.FirstName);
        model.Seller.LastName.Should().Be(searchModel.LastName);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsGetManyAsyncAndReturnsView(SellerSearchModel searchModel, IReadOnlyList<SellerEntity> sellers)
    {
        //  Arrange
        SellerService.Setup(_ => _.GetManyAsync(searchModel.FirstName, searchModel.LastName))
            .ReturnsAsync(sellers);

        //  Act
        IActionResult result = await Sut.SearchForAcceptance(searchModel);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewName.Should().Be(nameof(SellerController.CreateForAcceptance));
        view.ViewData.ModelState.IsValid.Should().BeTrue();
        
        SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
        model.FoundSellers.Should().BeEquivalentTo(sellers);
        model.HasSearched.Should().BeTrue();
        model.Seller.Should().NotBeNull();
        model.Seller.FirstName.Should().Be(searchModel.FirstName);
        model.Seller.LastName.Should().Be(searchModel.LastName);

        SellerService.Verify(_ => _.GetManyAsync(searchModel.FirstName, searchModel.LastName), Times.Once);
        VerifyNoOtherCalls();
    }
}
