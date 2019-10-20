using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        public ProductsTransaction Sale { get; set; }
        public bool ShowSuccess { get; set; }
        public bool OpenDocument { get; set; }

        public async Task OnGetAsync(int basarId, int saleId, bool? showSuccess = null,  bool? openDocument = null)
        {
            await LoadBasarAsync(basarId);

            ShowSuccess = showSuccess ?? false;
            OpenDocument = openDocument ?? false;

            Sale = await Context.Transactions.GetAsync(saleId);
        }
    }
}
