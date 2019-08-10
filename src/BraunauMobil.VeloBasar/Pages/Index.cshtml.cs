using BraunauMobil.VeloBasar.Data;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages
{
    public class IndexModel : BasarPageModel
    {
        public IndexModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);
        }
    }
}
