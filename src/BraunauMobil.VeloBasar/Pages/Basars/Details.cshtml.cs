using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DetailsParameter
    {
        public int? BasarId { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly IStatisticContext _statisticContext;

        public DetailsModel(IVeloContext context, IStatisticContext statisticContext)
        {
            _context = context;
            _statisticContext = statisticContext;
        }

        public BasarStatistic BasarStatistic { get; set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            int basarId;
            if (parameter.BasarId == null)
            {
                basarId = _context.Basar.Id;
            }
            else
            {
                basarId = parameter.BasarId.Value;
            }
            BasarStatistic = await _statisticContext.GetBasarStatisticAsnyc(basarId);
        }
    }
}
