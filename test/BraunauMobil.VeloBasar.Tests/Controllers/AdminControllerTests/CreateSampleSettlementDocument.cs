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
        A.CallTo(() => AdminService.CreateSampleSettlementDocumentAsync()).Returns(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleSettlementDocument();

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);
        }

        A.CallTo(() => AdminService.CreateSampleSettlementDocumentAsync()).MustHaveHappenedOnceExactly();
    }
}
