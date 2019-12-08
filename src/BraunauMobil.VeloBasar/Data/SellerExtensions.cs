using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraunauMobil.VeloBasar.Data
{
    public static class SellerExtensions
    {
        public static IQueryable<Seller> IncludeAll(this IQueryable<Seller> sellers)
        {
            return sellers
                .Include(s => s.Country);
        }
    }
}
