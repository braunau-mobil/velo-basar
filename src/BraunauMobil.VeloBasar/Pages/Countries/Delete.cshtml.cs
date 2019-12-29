using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Countries
{
    public class DeleteParameter
    {
        public int CountryId { get; set; }
        public int PageIndex { get; set; }
    }
    public class DeleteModel : PageModel
    {
        private readonly ICountryContext _context;

        public DeleteModel(ICountryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(DeleteParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (await _context.ExistsAsync(parameter.CountryId))
            {
                await _context.DeleteAsync(parameter.CountryId);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = parameter.PageIndex });
        }
    }
}
