using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class SelectOrCreateModel : BasarPageModel
    {
        public SelectOrCreateModel(VeloBasarContext context) : base(context)
        {
        }

        public IList<Basar> Basars { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (await Context.Basar.AnyAsync())
            {
                Basars = await Context.Basar.ToListAsync();
                return Page();
            }

            return RedirectToPage("./Create");
        }
    }
}
