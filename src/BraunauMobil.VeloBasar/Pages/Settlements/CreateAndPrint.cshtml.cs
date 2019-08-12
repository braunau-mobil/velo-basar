using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Settlements
{
    public class CreateAndPrintModel : BasarPageModel
    {
        public CreateAndPrintModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            var settlement = await Context.SettleSellerAsync(basarId, sellerId);

            var fileStore = await Context.GenerateSettlementDocIfNotExistAsync(settlement);

            return File(fileStore.Data, fileStore.ContentType);
        }
    }
}