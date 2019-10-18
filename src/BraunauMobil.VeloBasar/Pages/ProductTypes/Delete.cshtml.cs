using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class DeleteModel : BasarPageModel
    {
        public DeleteModel(VeloBasarContext context)  : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int ProductTypeId, int pageIndex, int? basarId)
        {
            if (await Context.ProductTypes.ExistsAsync(ProductTypeId))
            {
                await Context.DeleteProductType(ProductTypeId);
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/ProductTypes/List", new { pageIndex, basarId });
        }
    }
}
