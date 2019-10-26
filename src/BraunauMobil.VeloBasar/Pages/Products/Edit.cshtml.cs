using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class EditModel : BasarPageModel
    {
        public EditModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int productId)
        {
            await LoadBasarAsync(basarId);
            ViewData["Brands"] = new SelectList(Context.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(Context.ProductTypes, "Id", "Name");

            Product = await Context.Product.FirstOrDefaultAsync(p => p.Id == productId);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int basarId, string target)
        {
            await LoadBasarAsync(basarId);
            ViewData["Brands"] = new SelectList(Context.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(Context.ProductTypes, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(Product).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Context.Product.ExistsAsync(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect(target);
        }
    }
}
