using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class DeleteModel : BasarPageModel
    {
        public DeleteModel(VeloBasarContext context)  : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int brandId, int pageIndex, int? basarId)
        {
            if (await Context.ExistsBrand(brandId))
            {
                await Context.DeleteBrand(brandId);
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/Brands/List", new { pageIndex, basarId });
        }
    }
}
