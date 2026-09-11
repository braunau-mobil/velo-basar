using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class ScanQr
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task SuccessfulImport_ReturnsViewWithValidModelState(ProductEntity product, string qrScanText)
    {
        //  Arrange
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => QrImportService.TryApplyToProductAsync(qrScanText, product)).Returns(QrImportResult.Success());
        A.CallTo(() => AcceptProductService.GetAsync(product.SessionId, product)).Returns(model);

        //  Act
        IActionResult result = await Sut.ScanQr(product, qrScanText);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CreateEdit");
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            view.Model.Should().Be(model);
        }

        A.CallTo(() => QrImportService.TryApplyToProductAsync(qrScanText, product)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task EmptyQrText_ReturnsViewWithInvalidModelState(ProductEntity product, string qrScanText)
    {
        //  Arrange
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => QrImportService.TryApplyToProductAsync(qrScanText, product)).Returns(QrImportResult.Empty());
        A.CallTo(() => AcceptProductService.GetAsync(product.SessionId, product)).Returns(model);

        //  Act
        IActionResult result = await Sut.ScanQr(product, qrScanText);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task ChecksumMismatch_ReturnsViewWithInvalidModelState(ProductEntity product, string qrScanText)
    {
        //  Arrange
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => QrImportService.TryApplyToProductAsync(qrScanText, product)).Returns(QrImportResult.ChecksumMismatch());
        A.CallTo(() => AcceptProductService.GetAsync(product.SessionId, product)).Returns(model);

        //  Act
        IActionResult result = await Sut.ScanQr(product, qrScanText);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task UnmatchedProductType_ReturnsViewWithTypeIdModelError(ProductEntity product, string qrScanText, string productType)
    {
        //  Arrange
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => QrImportService.TryApplyToProductAsync(qrScanText, product)).Returns(QrImportResult.Success(unmatchedProductType: productType));
        A.CallTo(() => AcceptProductService.GetAsync(product.SessionId, product)).Returns(model);

        //  Act
        IActionResult result = await Sut.ScanQr(product, qrScanText);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState[nameof(ProductEntity.TypeId)]!.Errors.Should().NotBeEmpty();
    }
}
