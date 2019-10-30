using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class EditParameter
    {
        public int SellerId { get; set; }
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
        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            ViewData["Countries"] = new SelectList(_context.Country, "Id", "Name");

            Seller = await _context.Seller.FirstOrDefaultAsync(m => m.Id == parameter.SellerId);

            if (Seller == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Seller).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Seller.ExistsAsync(Seller.Id))
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
