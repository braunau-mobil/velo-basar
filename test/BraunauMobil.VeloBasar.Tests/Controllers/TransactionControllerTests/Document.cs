﻿using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Document
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ReturnsFileData(int id)
    {
        //  Arrange
        FileDataEntity fileData = Fixture.Create<FileDataEntity>();
        A.CallTo(() => TransactionService.GetDocumentAsync(id)).Returns(fileData);

        //  Act
        IActionResult result = await Sut.Document(id);

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeSameAs(fileData.Data);
            fileContent.FileDownloadName.Should().Be(fileData.FileName);
        }

        A.CallTo(() => TransactionService.GetDocumentAsync(id)).MustHaveHappenedOnceExactly();
    }
}
