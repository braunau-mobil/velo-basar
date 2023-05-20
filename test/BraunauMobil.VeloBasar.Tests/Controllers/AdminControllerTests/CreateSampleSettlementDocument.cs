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
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.CreateSampleSettlementDocumentAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
}
