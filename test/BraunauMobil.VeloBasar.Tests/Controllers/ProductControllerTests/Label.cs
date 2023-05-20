using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Label
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WithId_CallsGetLabelAndReturnsFileResult(int productId)
    {
        //  Arrange
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        ProductService.Setup(_ => _.GetLabelAsync(productId))
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.Label(productId);

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileDownloadName.Should().Be(fileData.FileName);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);

        ProductService.Verify(_ => _.GetLabelAsync(productId), Times.Once());
        VerifyNoOtherCalls();
    }
}
