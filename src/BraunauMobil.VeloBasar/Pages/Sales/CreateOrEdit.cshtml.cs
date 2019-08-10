using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CreateOrEditModel : BasarPageModel
    {
        private const int PageSize = 10;

        public CreateOrEditModel(VeloBasarContext context) : base(context)
        {
        }

        public Sale Sale { get; set; }

        [BindProperty]
        public PaginatedList<Product> Products { get; set; }

        [BindProperty]
        public int ProductId { get; set; }

        public async Task OnGetAsync(int basarId, int? saleId, int? pageIndex)
        {
            await LoadBasarAsync(basarId);

            if (saleId != null)
            {
                Products = await PaginatedList<Product>.CreateAsync(Context.Sale.Where(s => s.Id == saleId).SelectMany(s => s.Products).Select(ps => ps.Product).AsNoTracking(), pageIndex ?? 1, PageSize);
            }
            else
            {
                Products = new PaginatedList<Product>();
            }
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int? saleId, int? pageIndex)
        {
            var product = await Context.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
            if (product == null)
            {
                //  @todo!!
                return Page();
            }
            if (product.Status != ProductStatus.Available)
            {
                //  @todo
                return Page();
            }

            var sale = await Context.CreateOrGetSaleAsync(basarId, saleId);
            await Context.AddProductToSaleAsync(sale, product);

            return RedirectToPage("/Sales/CreateOrEdit", new { basarId, saleId = sale.Id, pageIndex });
        }
    }
}