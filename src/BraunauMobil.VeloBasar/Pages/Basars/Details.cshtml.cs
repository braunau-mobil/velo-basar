using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DetailsParameter
    {
        public int BasarId { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public DetailsModel(VeloBasarContext context)
        {
            _context = context;
        }

        public BasarStatistic BasarStatistic { get; set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            BasarStatistic = await _context.GetBasarStatisticAsnyc(parameter.BasarId);
        }
    }
}
