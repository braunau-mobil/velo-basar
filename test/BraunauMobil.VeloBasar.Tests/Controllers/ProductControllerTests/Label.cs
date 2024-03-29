﻿using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Label
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithId_CallsGetLabelAndReturnsFileResult(int productId)
    {
        //  Arrange
        FileDataEntity fileData = Fixture.Create<FileDataEntity>();
        A.CallTo(() => ProductService.GetLabelAsync(productId)).Returns(fileData);

        //  Act
        IActionResult result = await Sut.Label(productId);

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileDownloadName.Should().Be(fileData.FileName);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        }

        A.CallTo(() => ProductService.GetLabelAsync(productId)).MustHaveHappenedOnceExactly();
    }
}
