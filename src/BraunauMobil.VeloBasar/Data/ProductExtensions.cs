using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> DefaultOrder(this IQueryable<Product> products) => products.OrderById();
        public static IQueryable<Product> IncludeAll(this IQueryable<Product> products)
        {
            return products
                .Include(p => p.Brand)
                .Include(p => p.Type);
        }
    }
}
