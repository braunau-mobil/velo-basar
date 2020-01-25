using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class SetStateParameter
    {
        public int BrandId { get; set; }
        public ObjectState State { get; set; }
        public int PageIndex { get; set; }
    }
    public class SetStateModel : PageModel
    {
        private readonly IBrandContext _context;

        public SetStateModel(IBrandContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(SetStateParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (await _context.ExistsAsync(parameter.BrandId))
            {
                var brand = await _context.GetAsync(parameter.BrandId);
                brand.State = parameter.State;
                await _context.UpdateAsync(brand);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
