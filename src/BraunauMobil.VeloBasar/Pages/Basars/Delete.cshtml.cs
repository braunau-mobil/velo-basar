using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DeleteParameter
    {
        public int BasarToDeleteId { get; set; }
        public int PageIndex { get; set; }
    }
    public class DeleteModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public DeleteModel(VeloBasarContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(DeleteParameter parameter)
        {
            if (await _context.Basar.ExistsAsync(parameter.BasarToDeleteId))
            {
                await _context.DeleteBasarAsync(parameter.BasarToDeleteId);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = parameter.PageIndex });
        }
    }
}
