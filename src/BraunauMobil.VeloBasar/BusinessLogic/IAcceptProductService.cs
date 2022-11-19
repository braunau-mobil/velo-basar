namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IAcceptProductService
{
    Task<AcceptProductModel> CreateNewAsync(int sessionId);

    Task CreateAsync(ProductEntity product);

    Task DeleteAsync(int productId);

    Task<AcceptProductModel> GetAsync(int productId);

    Task<AcceptProductModel> GetAsync(int sessionId, ProductEntity product);

    Task UpdateAsync(ProductEntity product);
}
