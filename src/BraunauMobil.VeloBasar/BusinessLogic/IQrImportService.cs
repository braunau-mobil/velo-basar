namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IQrImportService
{
    Task<QrImportResult> TryApplyToSellerAsync(string? qrText, SellerEntity seller);

    Task<QrImportResult> TryApplyToProductAsync(string? qrText, ProductEntity product);
}
