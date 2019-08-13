using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CreateAndPrintModel : BasarPageModel
    {
        public CreateAndPrintModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int saleId)
        {
            await LoadBasarAsync(basarId);

            var fileStore = await Context.GenerateSaleDocIfNotExistAsync(Basar, saleId);

            return File(fileStore.Data, fileStore.ContentType);
        }
    }
}