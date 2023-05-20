using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class ExportSellersForNewsletter
    : TestBase
{
    [Fact]
    public async Task CallsExportSellersForNewsletterAsync_AndReturnsFileData()
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.ExportSellersForNewsletterAsCsvAsync())
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.ExportSellersForNewsletter();

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.ExportSellersForNewsletterAsCsvAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
}
