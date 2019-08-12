using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        public bool CanSettle { get; set; }

        public Seller Seller { get; set; }

        public SellerStatistics Stats { get; set; }

        public IList<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            Seller = await Context.Seller.Include(s => s.Country).FirstOrDefaultAsync(m => m.Id == sellerId);
            if (Seller == null)
            {
                return NotFound();
            }

            Products = await Context.GetProductsForSeller(basarId, sellerId).AsNoTracking().ToListAsync();
            Stats = await Context.GetSellerStatisticsAsync(Seller.Id);

            CanSettle = Products.NotSold().Any();

            return Page();
        }

        public IDictionary<string, string> GetItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("sellerId", Seller.Id.ToString());
            route.Add("productId", product.Id.ToString());
            return route;
        }

        public IDictionary<string, string> GetSellerRoute()
        {
            var route = GetRoute();
            route.Add("sellerId", Seller.Id.ToString());
            return route;
        }
    }
}
