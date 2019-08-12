using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Linq;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        public Seller Seller { get; set; }

        public SellerStatistics Stats { get; set; }

        public PaginatedList<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int? basarId, int? sellerId, int? pageIndex)
        {
            await LoadBasarAsync(basarId);

            if (sellerId == null)
            {
                return NotFound();
            }

            Seller = await Context.Seller.Include(s => s.Country).FirstOrDefaultAsync(m => m.Id == sellerId);
            if (Seller == null)
            {
                return NotFound();
            }

            var pageSize = 10;
            Products = await PaginatedList<Product>.CreateAsync(Context.Acceptance.Where(a => a.SellerId == Seller.Id).SelectMany(a => a.Products).Select(pa => pa.Product).AsNoTracking(), pageIndex ?? 1, pageSize);

            Stats = await Context.GetSellerStatisticsAsync(Seller.Id);

            return Page();
        }
    }
}
