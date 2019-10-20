using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages
{
    public class ShowFileModel : BasarPageModel
    {
        public ShowFileModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int fileId)
        {
            await LoadBasarAsync(basarId);

            var file = await Context.FileStore.GetAsync(fileId);
            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType);
        }
    }
}