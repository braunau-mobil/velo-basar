using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public static class IModelExtensions
    {
        public static async Task<bool> ExistsAsync<T>(this IQueryable<T> models, int id) where T : IModel
        {
            return await models.AnyAsync(p => p.Id == id);
        }
        public static IQueryable<T> OrderById<T>(this IQueryable<T> models) where T : IModel
        {
            return models.OrderBy(p => p.Id);
        }
    }
}
