using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class EditParameter
    {
        public int ProductId { get; set; }
        public string Target { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public EditModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            ViewData["Brands"] = new SelectList(_context.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "Name");

            Product = await _context.Product.FirstOrDefaultAsync(p => p.Id == parameter.ProductId);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            ViewData["Brands"] = new SelectList(_context.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Product.ExistsAsync(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect(parameter.Target);
        }
    }
}
