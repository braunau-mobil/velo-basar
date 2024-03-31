using BraunauMobil.VeloBasar.Parameters;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ISellerService
    : ICrudService<SellerEntity, SellerListParameter>
{
    Task<bool> ExistsAsync(int id);

    Task<SellerDetailsModel> GetDetailsAsync(int basarId, int sellerId);

    Task<FileDataEntity> GetLabelsAsync(int basarId, int sellerId);

    Task<IReadOnlyList<SellerEntity>> GetManyAsync(string firstName, string lastName);

    Task<int> SettleAsync(int basarId, int sellerId);

    Task TriggerStatusPushAsync(int basarId, int sellerId);
}
