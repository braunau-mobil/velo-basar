using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int productId)
        {
            await LoadBasarAsync(basarId);

            Product = await Context.Product.FirstOrDefaultAsync(p => p.Id == productId);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public override IDictionary<string, string> GetRoute()
        {
            var route = base.GetRoute();
            route.Add("productId", Product.Id.ToString());
            return route;
        }
    }
}
