using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class CreateSampleSettlementDocument
    : TestBase
{
    [Fact]
    public async Task CallsCreateSampleSettlementDocumentAsync_AndReturnsFileData()
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.CreateSampleSettlementDocumentAsync())
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleSettlementDocument();

        //  Assert
        result.Should().NotBeNull();
        FileContentResult fileContentResult = result.Should().BeOfType<FileContentResult>().Subject;
        fileContentResult.ContentType.Should().Be(fileData.ContentType);
        fileContentResult.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContentResult.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.CreateSampleSettlementDocumentAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
}
