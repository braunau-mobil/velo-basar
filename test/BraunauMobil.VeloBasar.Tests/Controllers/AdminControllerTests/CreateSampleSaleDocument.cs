using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class CreateSampleSaleDocument
    : TestBase
{
    [Fact]
    public async Task CallsCreateSampleSaleDocumentAsync_AndReturnsFileData()
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.CreateSampleSaleDocumentAsync())
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleSaleDocument();

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.CreateSampleSaleDocumentAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
}
