using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DeleteModel : BasarPageModel
    {
        public DeleteModel(VeloBasarContext context)  : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarToDeleteId, int pageIndex, int? basarId)
        {
            if (await Context.Basar.ExistsAsync(basarToDeleteId))
            {
                await Context.DeleteBasarAsync(basarToDeleteId);
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/Basars/List", new { pageIndex, basarId });
        }
    }
}
