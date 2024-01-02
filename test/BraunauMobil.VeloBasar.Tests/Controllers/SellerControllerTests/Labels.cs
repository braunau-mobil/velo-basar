using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class Labels
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task CallsGetLabelsAndReturnsFile(int basarId, int sellerId)
    {
        //  Arrange
        FileDataEntity fileData = Fixture.Create<FileDataEntity>();
        A.CallTo(() => SellerService.GetLabelsAsync(basarId, sellerId)).Returns(fileData);

        //  Act
        IActionResult result = await Sut.Labels(basarId, sellerId);

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().Be(fileData.FileName);
        }

        A.CallTo(() => SellerService.GetLabelsAsync(basarId, sellerId)).MustHaveHappenedOnceExactly();
    }
}
