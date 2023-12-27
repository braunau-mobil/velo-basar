using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Document
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ReturnsFileData(int id)
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        A.CallTo(() => TransactionService.GetDocumentAsync(id)).Returns(fileData);

        //  Act
        IActionResult result = await Sut.Document(id);

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeSameAs(fileData.Data);
        fileContent.FileDownloadName.Should().Be(fileData.FileName);

        A.CallTo(() => TransactionService.GetDocumentAsync(id)).MustHaveHappenedOnceExactly();
    }
}
