﻿using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class CreateSampleSaleDocument
    : TestBase
{
    [Fact]
    public async Task CallsCreateSampleSaleDocumentAsync_AndReturnsFileData()
    {
        //  Arrange
        FileDataEntity fileData = Fixture.Create<FileDataEntity>();
        A.CallTo(() => AdminService.CreateSampleSaleDocumentAsync()).Returns(fileData);

        //  Act
        IActionResult result = await Sut.CreateSampleSaleDocument();

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);
        }

        A.CallTo(() => AdminService.CreateSampleSaleDocumentAsync()).MustHaveHappenedOnceExactly();
    }
}
