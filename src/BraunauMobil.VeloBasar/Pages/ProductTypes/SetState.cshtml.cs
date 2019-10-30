using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class SetStateParameter
    {
        public int ProductTypeId { get; set; }
        public ObjectState State { get; set; }
        public int PageIndex { get; set; }
    }
    public class SetStateModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public SetStateModel(VeloBasarContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(SetStateParameter parameter)
        {
            if (await _context.ProductTypes.ExistsAsync(parameter.ProductTypeId))
            {
                var productType = await _context.ProductTypes.GetAsync(parameter.ProductTypeId);
                productType.State = parameter.State;
                _context.Attach(productType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = parameter.PageIndex });
        }
    }
}
