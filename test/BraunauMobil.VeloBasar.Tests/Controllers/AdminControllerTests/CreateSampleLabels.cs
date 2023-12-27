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
        A.CallTo(() => AdminService.CreateSampleLabelsAsync()).Returns(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleLabels();

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);
        }

        A.CallTo(() => AdminService.CreateSampleLabelsAsync()).MustHaveHappenedOnceExactly();
    }
}
