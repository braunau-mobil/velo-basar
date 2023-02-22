using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AdminController
    : AbstractVeloController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
    }

    [Authorize]
    public async Task<IActionResult> CreateSampleAcceptanceDocument()
    {
        FileDataEntity fileData = await _adminService.CreateSampleAcceptanceDocumentAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    [Authorize]
    public async Task<IActionResult> CreateSampleLabels()
    {
        FileDataEntity fileData = await _adminService.CreateSampleLabelsAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    [Authorize]
    public async Task<IActionResult> CreateSampleSaleDocument()
    {
        FileDataEntity fileData = await _adminService.CreateSampleSaleDocumentAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    [Authorize]
    public async Task<IActionResult> CreateSampleSettlementDocument()
    {
        FileDataEntity fileData = await _adminService.CreateSampleSettlementDocumentAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    [Authorize]
    public IActionResult Export()
        => View();

    [Authorize]
    public async Task<IActionResult> ExportSellersForNewsletter()
    {
        FileDataEntity fileData = await _adminService.ExportSellersForNewsletterAsCsvAsync();
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    [Authorize]
    public IActionResult PrintTest()
        => View();
}
