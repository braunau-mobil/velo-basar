using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class SelectModel : BasarPageModel
    {
        public SelectModel(VeloBasarContext context) : base(context)
        {
        }

        public IList<Basar> Basars { get; set; }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);

            Basars = await Context.Basar.GetEnabled().ToListAsync();
        }
    }
}
