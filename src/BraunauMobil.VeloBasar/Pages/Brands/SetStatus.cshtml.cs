using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class SetStatusModel : BasarPageModel
    {
        public SetStatusModel(VeloBasarContext context)  : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int brandId, ModelStatus status, int pageIndex, int? basarId)
        {
            if (await Context.Brand.ExistsAsync(brandId))
            {
                var brand = await Context.Brand.GetAsync(brandId);
                brand.Status = status;
                Context.Attach(brand).State = EntityState.Modified;
                await Context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/Brands/List", new { pageIndex, basarId });
        }
    }
}
