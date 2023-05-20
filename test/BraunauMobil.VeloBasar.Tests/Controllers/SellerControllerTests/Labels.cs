using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class Labels
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsGetLabelsAndReturnsFile(int basarId, int sellerId)
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        SellerService.Setup(_ => _.GetLabelsAsync(basarId, sellerId))
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.Labels(basarId, sellerId);

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().Be(fileData.FileName);

        SellerService.Verify(_ => _.GetLabelsAsync(basarId, sellerId), Times.Once());
        VerifyNoOtherCalls();
    }
}
