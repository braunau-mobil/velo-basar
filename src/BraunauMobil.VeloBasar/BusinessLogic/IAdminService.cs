namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IAdminService
{
    Task<FileDataEntity> CreateSampleAcceptanceDocumentAsync();

    Task<FileDataEntity> CreateSampleLabelsAsync();

    Task<FileDataEntity> CreateSampleSaleDocumentAsync();

    Task<FileDataEntity> CreateSampleSettlementDocumentAsync();
}
