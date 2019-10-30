using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class EditParameter
    {
        public int BasarToEditId { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly VeloBasarContext _context;
        private EditParameter _parameter;

        public EditModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Basar BasarToEdit { get; set; }

        public async Task OnGetAsync(EditParameter parameter)
        {
            _parameter = parameter;
            BasarToEdit = await _context.Basar.GetAsync(parameter.BasarToEditId);
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            _parameter = parameter;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BasarToEdit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = parameter.PageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new ListParameter { PageIndex = _parameter.PageIndex });
    }
}
