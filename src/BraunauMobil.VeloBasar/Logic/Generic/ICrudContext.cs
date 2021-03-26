using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic.Generic
{
    public interface ICrudContext<TModel> where TModel : IModel
    {
        Task<bool> CanDeleteAsync(TModel item);
        Task<TModel> CreateAsync(TModel item);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<TModel> GetAsync(int id);
        IQueryable<TModel> GetMany(string searchString);
        SelectList GetSelectList();
        SelectList GetSelectListWithAllItem();
        Task UpdateAsync(TModel item);
    }
}
