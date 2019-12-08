using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Linq;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ProductTypeExtensions
    {
        public static IQueryable<ProductType> DefaultOrder(this IQueryable<ProductType> productTypes) => productTypes.OrderById();
    }
}
