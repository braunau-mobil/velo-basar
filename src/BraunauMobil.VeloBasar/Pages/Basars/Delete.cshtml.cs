using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DeleteParameter
    {
        public int BasarToDeleteId { get; set; }
        public int PageIndex { get; set; }
    }
    public class DeleteModel : PageModel
    {
        private readonly IBasarContext _context;

        public DeleteModel(IBasarContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(DeleteParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (await _context.ExistsAsync(parameter.BasarToDeleteId))
            {
                await _context.DeleteAsync(parameter.BasarToDeleteId);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
