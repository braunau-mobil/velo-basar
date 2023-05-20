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
        TransactionService.Setup(ts => ts.GetAcceptanceLabelsAsync(id))
            .ReturnsAsync(fileData);
        
        // Act
        IActionResult result = await Sut.Download(id);

        // Assert
        result.Should().NotBeNull();
        
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().Be(fileData.FileName);

        TransactionService.Verify(ts => ts.GetAcceptanceLabelsAsync(id), Times.Once);
    }
}
