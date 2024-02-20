using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Controllers;

[Authorize]
public sealed class AdminController
    : AbstractVeloController
{
    private readonly IAdminService _adminService;
    private readonly IClock _clock;

    public AdminController(IAdminService adminService, IClock clock)
    {
        _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public async Task<IActionResult> CreateSampleAcceptanceDocument()
    {
        FileDataEntity fileData = await _adminService.CreateSampleAcceptanceDocumentAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public async Task<IActionResult> CreateSampleLabels()
    {
        FileDataEntity fileData = await _adminService.CreateSampleLabelsAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public async Task<IActionResult> CreateSampleSaleDocument()
    {
        FileDataEntity fileData = await _adminService.CreateSampleSaleDocumentAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public async Task<IActionResult> CreateSampleSettlementDocument()
    {
        FileDataEntity fileData = await _adminService.CreateSampleSettlementDocumentAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public IActionResult Export()
    {
        ExportModel model = new()
        {
            MinPermissionDate = _clock.GetCurrentDate()
        };
        return View(model);
    }

    public async Task<IActionResult> ExportSellersForNewsletter(ExportModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        DateTime? minPermissionTimestamp = null;
        if (model.UseMinPermissionDate)
        {
            minPermissionTimestamp = model.MinPermissionDate.ToDateTime(TimeOnly.MinValue);
        }

        FileDataEntity fileData = await _adminService.ExportSellersForNewsletterAsCsvAsync(minPermissionTimestamp);
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public IActionResult PrintTest()
        => View();
}
