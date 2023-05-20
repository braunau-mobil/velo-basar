using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class CreateSampleLabels
    : TestBase
{
    [Fact]
    public async Task CallsCreateSampleLabelsAsync_AndReturnsFileData()
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.CreateSampleLabelsAsync())
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleLabels();

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.CreateSampleLabelsAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
}
