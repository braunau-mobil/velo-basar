using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Countries
{
    public class EditParameter
    {
        public int CountryId { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly ICountryContext _context;
        private int _pageIndex;

        public EditModel(ICountryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Country Country { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            Country = await _context.GetAsync(parameter.CountryId);
            _pageIndex = parameter.PageIndex;

            if (Country == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateAsync(Country);
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
    }
}
