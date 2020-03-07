using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IProductContext
    {
        Task<bool> ExistsAsync(int id);
        Task<Product> GetAsync(int id);
        IQueryable<Product> GetMany(IReadOnlyList<int> ids);
        IQueryable<Product> GetProductsForBasar(Basar basar);
        IQueryable<Product> GetProductsForBasar(Basar basar, string searchString, StorageState? storageState, ValueState? valueState, int? brand, int? productType);
        IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId);
        Task<IReadOnlyList<Product>> InsertProductsAsync(Basar basar, Seller seller, IReadOnlyList<Product> products);
        Task ReloadRelationsAsync(IReadOnlyList<Product> products);
        Task UpdateAsync(Product product);
    }
}
