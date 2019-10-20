using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        public ProductsTransaction Acceptance { get; set; }
        public bool ShowSuccess { get; set; }
        public bool OpenDocument { get; set; }

        public async Task OnGetAsync(int basarId, int acceptanceId, bool? showSuccess = null, bool? openDocument = null)
        {
            await LoadBasarAsync(basarId);

            ShowSuccess = showSuccess ?? false;
            OpenDocument = openDocument ?? false;

            Acceptance = await Context.Transactions.GetAsync(acceptanceId);
        }
    }
}
