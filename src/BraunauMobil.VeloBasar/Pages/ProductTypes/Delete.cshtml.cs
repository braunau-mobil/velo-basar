using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class DeleteParameter
    {
        public int ProductTypeId { get; set; }
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
            if (await _context.ProductTypes.ExistsAsync(parameter.ProductTypeId))
            {
                await _context.DeleteProductType(parameter.ProductTypeId);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = parameter.PageIndex });
        }
    }
}
