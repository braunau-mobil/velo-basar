using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class SetStateParameter
    {
        public int ProductTypeId { get; set; }
        public ObjectState State { get; set; }
        public int PageIndex { get; set; }
    }
    public class SetStateModel : PageModel
    {
        private readonly IProductTypeContext _context;

        public SetStateModel(IProductTypeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(SetStateParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (await _context.ExistsAsync(parameter.ProductTypeId))
            {
                await _context.SetStateAsync(parameter.ProductTypeId, parameter.State);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
