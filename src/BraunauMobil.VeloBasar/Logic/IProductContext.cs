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
        IQueryable<Product> GetMany(IList<int> ids);
        IQueryable<Product> GetProductsForBasar(Basar basar);
        IQueryable<Product> GetProductsForBasar(Basar basar, string searchString, StorageState? storageState, ValueState? valueState);
        IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId);
        Task UpdateAsync(Product product);
        void AttachRelations(IList<Product> products);
    }
}
