using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IProductService
{
    Task<bool> ExistsForBasarAsync(int basarId, int productId);

    Task<ProductEntity?> FindAsync(int id);

    Task<ProductEntity> GetAsync(int id);

    Task<ProductDetailsModel> GetDetailsAsync(int activeBasarId, int productId);

    Task<FileDataEntity> GetLabelAsync(int productId);

    Task<IReadOnlyList<ProductEntity>> GetManyAsync(IList<int> ids);

    Task<IPaginatedList<ProductEntity>> GetManyAsync(int pageSize, int pageIndex, int activeBasarId, string searchString, StorageState? storageState, ValueState? valueState, string? brand, int? productTypeId);

    Task LockAsync(int id, string notes);

    Task SetLostAsync(int id, string notes);

    Task UnlockAsync(int id, string notes);

    Task UpdateAsync(ProductEntity product);
}
