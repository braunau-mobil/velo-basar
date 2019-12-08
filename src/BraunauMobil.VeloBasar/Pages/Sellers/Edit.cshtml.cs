using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class EditParameter
    {
        public int SellerId { get; set; }
        public string Target { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly ISellerContext _sellerContext;
        private readonly ICountryContext _countryContext;

        public EditModel(ISellerContext sellerContext, ICountryContext countryContext)
        {
            _sellerContext = sellerContext;
            _countryContext = countryContext;
        }

        [BindProperty]
        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            ViewData["Countries"] = _countryContext.GetSelectList();

            Seller = await _sellerContext.GetAsync(parameter.SellerId);

            if (Seller == null)
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

            await _sellerContext.UpdateAsync(Seller);
            return Redirect(parameter.Target);
        }
    }
}
