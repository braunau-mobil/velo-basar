using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class SelectBasarModel : BasarPageModel
    {
        public SelectBasarModel(VeloBasarContext context) : base(context)
        {
        }

        public IList<Basar> Basars { get; set; }

        public override async Task<IActionResult> OnGetAsync(int? basarId)
        {
            Basars = await Context.Basar.ToListAsync();

            return await base.OnGetAsync(basarId);
        }
    }
}
