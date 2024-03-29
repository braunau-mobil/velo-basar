﻿using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class ExportSellersForNewsletter
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task DoNotUseDate_CallsExportSellersForNewsletterAsync_AndReturnsFileData(DateTime timestamp)
    {
        //  Arrange
        DateOnly minPermissionDate = DateOnly.FromDateTime(timestamp);
        DateTime? minPermissionTimestamp = minPermissionDate.ToDateTime(TimeOnly.MinValue);
        ExportModel model = new()
        {
            UseMinPermissionDate = false,
            MinPermissionDate = minPermissionDate
        };

        FileDataEntity fileData = Fixture.Create<FileDataEntity>();
        A.CallTo(() => AdminService.ExportSellersForNewsletterAsCsvAsync(null)).Returns(fileData);

        //  Act
        IActionResult result = await Sut.ExportSellersForNewsletter(model);

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);
        }

        A.CallTo(() => AdminService.ExportSellersForNewsletterAsCsvAsync(null)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task UseDate_CallsExportSellersForNewsletterAsync_AndReturnsFileData(DateTime timestamp)
    {
        //  Arrange
        DateOnly minPermissionDate = DateOnly.FromDateTime(timestamp);
        DateTime? minPermissionTimestamp = minPermissionDate.ToDateTime(TimeOnly.MinValue);
        ExportModel model = new()
        {
            UseMinPermissionDate = true,
            MinPermissionDate = minPermissionDate
        };
        
        FileDataEntity fileData = Fixture.Create<FileDataEntity>();
        A.CallTo(() => AdminService.ExportSellersForNewsletterAsCsvAsync(minPermissionTimestamp)).Returns(fileData);

        //  Act
        IActionResult result = await Sut.ExportSellersForNewsletter(model);

        //  Assert
        using (new AssertionScope())
        {
            FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
            fileContent.ContentType.Should().Be(fileData.ContentType);
            fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
            fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);
        }

        A.CallTo(() => AdminService.ExportSellersForNewsletterAsCsvAsync(minPermissionTimestamp)).MustHaveHappenedOnceExactly();
    }
}
