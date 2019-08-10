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

        public Acceptance Acceptance { get; set; }

        public async Task OnGetAsync(int basarId, int acceptanceId)
        {
            await LoadBasarAsync(basarId);

            Acceptance = await Context.GetAcceptanceAsync(acceptanceId);
        }
    }
}
