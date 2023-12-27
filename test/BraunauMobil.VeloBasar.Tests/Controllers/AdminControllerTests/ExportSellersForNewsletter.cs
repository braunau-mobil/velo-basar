using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class ExportSellersForNewsletter
    : TestBase
{
    [Theory]
    [AutoData]
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

        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.ExportSellersForNewsletterAsCsvAsync(null))
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.ExportSellersForNewsletter(model);

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.ExportSellersForNewsletterAsCsvAsync(null), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
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
        
        FileDataEntity fileData = Fixture.BuildFileDataEntity().Create();
        AdminService.Setup(_ => _.ExportSellersForNewsletterAsCsvAsync(minPermissionTimestamp))
            .ReturnsAsync(fileData);

        //  Act
        IActionResult result = await Sut.ExportSellersForNewsletter(model);

        //  Assert
        FileContentResult fileContent = result.Should().BeOfType<FileContentResult>().Subject;
        fileContent.ContentType.Should().Be(fileData.ContentType);
        fileContent.FileContents.Should().BeEquivalentTo(fileData.Data);
        fileContent.FileDownloadName.Should().BeEquivalentTo(fileData.FileName);

        AdminService.Verify(_ => _.ExportSellersForNewsletterAsCsvAsync(minPermissionTimestamp), Times.Once);
        VerifyNoOtherCalls();
    }
}
