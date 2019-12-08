using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IProductTypeContext
    {
        Task<bool> CanDeleteAsync(ProductType type);
        Task<ProductType> CreateAsync(ProductType type);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<ProductType> GetAsync(int id);
        IQueryable<ProductType> GetMany(string searchString);
        SelectList GetSelectList();
        Task SetStateAsync(int id, ObjectState state);
        Task UpdateAsync(ProductType toUpdate);
    }
}
