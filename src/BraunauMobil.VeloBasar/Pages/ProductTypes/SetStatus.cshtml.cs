using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class SetStatusModel : BasarPageModel
    {
        public SetStatusModel(VeloBasarContext context)  : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int productTypeId, ModelStatus status, int pageIndex, int? basarId)
        {
            if (await Context.ExistsProductType(productTypeId))
            {
                var productType = await Context.GetProductTypeAsync(productTypeId);
                productType.Status = status;
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
