using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

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

        public async Task OnGetAsync(EditParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            _parameter = parameter;
            BasarToEdit = await _context.GetAsync(parameter.BasarToEditId);
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            _parameter = parameter;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateAsync(BasarToEdit);
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new SearchAndPaginationParameter { PageIndex = _parameter.PageIndex });
    }
}
