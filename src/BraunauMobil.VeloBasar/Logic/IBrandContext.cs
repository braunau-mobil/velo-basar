using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IBrandContext
    {
        Task<bool> CanDeleteAsync(Brand item);
        Task CreateAsync(Brand brand);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Brand> GetAsync(int id);
        IQueryable<Brand> GetMany(string searchString);
        SelectList GetSelectList();
        Task UpdateAsync(Brand brand);
    }
}
