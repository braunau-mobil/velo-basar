using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Contracts;
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

        public DetailsModel(IVeloContext context)
        {
            _context = context;
        }

        public BasarStatistic BasarStatistic { get; set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            int basarId;
            if (parameter.BasarId == null)
            {
                basarId = _context.Basar.Id;
            }
            else
            {
                basarId = parameter.BasarId.Value;
            }
            BasarStatistic = await _context.Db.GetBasarStatisticAsnyc(basarId);
        }
    }
}
