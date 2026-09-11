using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class ScanQrForAcceptance
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task SuccessfulImport_ReturnsViewWithValidModelState(SellerEntity seller, string qrScanText)
    {
        //  Arrange
        A.CallTo(() => QrImportService.TryApplyToSellerAsync(qrScanText, seller)).Returns(QrImportResult.Success());

        //  Act
        IActionResult result = await Sut.ScanQrForAcceptance(seller, qrScanText);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be(nameof(SellerController.CreateForAcceptance));
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
            model.Seller.Should().Be(seller);
        }

        A.CallTo(() => QrImportService.TryApplyToSellerAsync(qrScanText, seller)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task EmptyQrText_ReturnsViewWithInvalidModelState(SellerEntity seller, string qrScanText)
    {
        //  Arrange
        A.CallTo(() => QrImportService.TryApplyToSellerAsync(qrScanText, seller)).Returns(QrImportResult.Empty());

        //  Act
        IActionResult result = await Sut.ScanQrForAcceptance(seller, qrScanText);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task ChecksumMismatch_ReturnsViewWithInvalidModelState(SellerEntity seller, string qrScanText)
    {
        //  Arrange
        A.CallTo(() => QrImportService.TryApplyToSellerAsync(qrScanText, seller)).Returns(QrImportResult.ChecksumMismatch());

        //  Act
        IActionResult result = await Sut.ScanQrForAcceptance(seller, qrScanText);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task UnmatchedCountry_ReturnsViewWithCountryIdModelError(SellerEntity seller, string qrScanText, string countryIso3)
    {
        //  Arrange
        A.CallTo(() => QrImportService.TryApplyToSellerAsync(qrScanText, seller)).Returns(QrImportResult.Success(unmatchedCountryIso3: countryIso3));

        //  Act
        IActionResult result = await Sut.ScanQrForAcceptance(seller, qrScanText);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState[nameof(SellerEntity.CountryId)]!.Errors.Should().NotBeEmpty();
    }
}
