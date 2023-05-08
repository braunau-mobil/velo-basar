using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class CreateSampleAcceptanceDocument
    : TestBase
{
    [Fact]
    public async Task CallsCreateSampleAcceptanceDocumentAsync_AndReturnsFileData()
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.CreateSampleAcceptanceDocumentAsync())
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleAcceptanceDocument();

        //  Assert
        result.Should().NotBeNull();
        FileContentResult fileContentResult = result.Should().BeOfType<FileContentResult>().Subject;
        fileContentResult.ContentType.Should().Be(fileData.ContentType);
        fileContentResult.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContentResult.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.CreateSampleAcceptanceDocumentAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
}
