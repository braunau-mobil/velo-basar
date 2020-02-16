using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class EditParameter
    {
        public int BasarToEditId { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly IBasarContext _context;
        private EditParameter _parameter;

        public EditModel(IBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Basar BasarToEdit { get; set; }
        [Display(Name = "Artikel Provision in Prozent")]
        [Range(0, 100, ErrorMessage = "Bitte einen Wert zwischen 0 und 100 % eingeben")]
        [BindProperty]
        public int ProductCommissionPercentage { get; set; }

        public async Task OnGetAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            _parameter = parameter;
            BasarToEdit = await _context.GetAsync(parameter.BasarToEditId);
            ProductCommissionPercentage = (int)(BasarToEdit.ProductCommission * 100);
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            _parameter = parameter;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            BasarToEdit.ProductCommission = ProductCommissionPercentage / 100.0m;
            await _context.UpdateAsync(BasarToEdit);
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new SearchAndPaginationParameter { PageIndex = _parameter.PageIndex });
    }
}
