using BraunauMobil.VeloBasar.Data;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class StartNewModel : BasarPageModel
    {
        public StartNewModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);
        }
    }
}
