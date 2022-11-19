using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ISellerService
    : ICrudService<SellerEntity>
{
    Task<bool> ExistsAsync(int id);

    Task<SellerDetailsModel> GetDetailsAsync(int basarId, int sellerId);

    Task<FileDataEntity> GetLabelsAsync(int basarId, int sellerId);

    Task<IReadOnlyList<SellerEntity>> GetManyAsync(string firstName, string lastName);

    Task<IPaginatedList<SellerEntity>> GetManyAsync(int pageSize, int pageIndex, string? searchString = null, ObjectState? objectState = null, ValueState? valueState = null);

    Task<int> SettleAsync(int basarId, int sellerId);
}
