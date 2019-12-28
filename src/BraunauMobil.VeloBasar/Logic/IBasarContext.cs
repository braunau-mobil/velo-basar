using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IBasarContext
    {
        Task<bool> CanDeleteAsync(Basar basar);
        Task<Basar> CreateAsync(Basar toCreate);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        bool Exists(int id);
        Task<Basar> GetAsync(int id);
        Basar GetSingle(int id);
        IQueryable<Basar> GetMany(string searchString = null);
        SelectList GetSelectList();
        bool HasBasars();
        Task UpdateAsync(Basar toUpdate);
    }
}
