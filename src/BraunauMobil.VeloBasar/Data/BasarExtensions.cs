using BraunauMobil.VeloBasar.Models;
using System.Linq;

namespace BraunauMobil.VeloBasar.Data
{
    public static class BasarExtensions
    {
        public static IQueryable<Basar> DefaultOrder(this IQueryable<Basar> basars)
        {
            return basars.OrderBy(b => b.Date);
        }
    }
}
