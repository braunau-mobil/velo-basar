using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DetailsModel : BasarPageModel
    {
        public DetailsModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);
        }
    }
}
