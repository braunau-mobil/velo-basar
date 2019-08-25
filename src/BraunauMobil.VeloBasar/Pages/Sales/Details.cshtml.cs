using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        public ProductsTransaction Sale { get; set; }

        public async Task OnGetAsync(int basarId, int saleId)
        {
            await LoadBasarAsync(basarId);

            Sale = await Context.GetSaleAsync(saleId);
        }
    }
}
