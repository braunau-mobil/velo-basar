using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ITransactionService
{
    Task<int> AcceptAsync(int basarId, int sellerId, IEnumerable<int> productIds);

    Task<int> CancelAsync(int basarId, int saleId, IEnumerable<int> productIds);

    Task<int> CheckoutAsync(int basarId, IEnumerable<int> productIds);

    Task<TransactionEntity?> FindAsync(int basarId, TransactionType type, int number);

    Task<FileDataEntity> GetAcceptanceLabelsAsync(int id);

    Task<TransactionEntity> GetAsync(int id);

    Task<TransactionEntity> GetAsync(int id, decimal amountGiven);

    Task<FileDataEntity> GetDocumentAsync(int id);

    Task<TransactionEntity> GetLatestForProductAsync(int productId);

    Task<IPaginatedList<TransactionEntity>> GetManyAsync(int pageSize, int pageIndex, int basarId, TransactionType? type = null, string? searchString = null);

    Task<IReadOnlyList<ProductEntity>> GetProductsToCancelAsync(int id);

    Task<int> LockAsync(int basarId, string? notes, int productId);

    Task<int> SetLostAsync(int basarId, string? notes, int productId);

    Task<int> SettleAsync(int basarId, int sellerId, IEnumerable<int> productIds);

    Task<int> UnlockAsync(int basarId, string? notes, int productId);

    Task<int> UnsettleAsync(int settlementId);
}
