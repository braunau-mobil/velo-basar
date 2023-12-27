using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptanceLabelsControllerTests;

public class Download
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ValidId_ReturnsFileContent(int id)
    {
        // Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        A.CallTo(() => TransactionService.GetAcceptanceLabelsAsync(id)).Returns(fileData);
        
        // Act
        IActionResult result = await Sut.Download(id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
        
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().Be(fileData.FileName);
        }

        A.CallTo(() => TransactionService.GetAcceptanceLabelsAsync(id)).MustHaveHappenedOnceExactly();
    }
}
