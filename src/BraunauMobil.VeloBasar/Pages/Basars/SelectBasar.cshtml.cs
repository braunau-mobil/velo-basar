using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class SelectBasarModel : BasarPageModel
    {
        public SelectBasarModel(VeloBasarContext context) : base(context)
        {
        }

        public IList<Basar> Basars { get; set; }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);

            Basars = await Context.Basar.ToListAsync();
        }
    }
}
