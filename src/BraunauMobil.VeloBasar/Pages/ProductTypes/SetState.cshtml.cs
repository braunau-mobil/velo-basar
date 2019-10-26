using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class SetStateModel : BasarPageModel
    {
        public SetStateModel(VeloBasarContext context)  : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int productTypeId, ObjectState state, int pageIndex, int? basarId)
        {
            if (await Context.ProductTypes.ExistsAsync(productTypeId))
            {
                var productType = await Context.ProductTypes.GetAsync(productTypeId);
                productType.State = state;
                Context.Attach(productType).State = EntityState.Modified;
                await Context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/ProductTypes/List", new { pageIndex, basarId });
        }
    }
}
