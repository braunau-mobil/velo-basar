using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CreateOrEditModel : BasarPageModel
    {
        public CreateOrEditModel(VeloBasarContext context) : base(context)
        {
        }

        public Sale Sale { get; set; }

        [BindProperty]
        public IList<Product> Products { get; set; }

        [BindProperty]
        public int ProductId { get; set; }

        public async Task OnGetAsync(int basarId, int? saleId)
        {
            await LoadBasarAsync(basarId);

            if (saleId != null)
            {
                Sale = await Context.Sale.FirstOrDefaultAsync(s => s.Id == saleId);
                Products = await Context.Sale.Where(s => s.Id == saleId).SelectMany(s => s.Products).Select(ps => ps.Product).AsNoTracking().ToListAsync();
            }
            else
            {
                Products = new PaginatedList<Product>();
            }
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int? saleId)
        {
            await LoadBasarAsync(basarId);

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

            var sale = await Context.AddProductToSaleAsync(basarId, saleId, product);

            return RedirectToPage("/Sales/CreateOrEdit", new { basarId, saleId = sale.Id });
        }
    }
}