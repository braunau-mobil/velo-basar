using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class EditModel : BasarPageModel
    {
        private int _pageIndex;

        public EditModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Basar BasarToEdit { get; set; }

        public async Task OnGetAsync(int basarToEditId, int pageIndex, int? basarId)
        {
            await LoadBasarAsync(basarId);
            _pageIndex = pageIndex;

            BasarToEdit = await Context.Basar.GetAsync(basarToEditId);
        }
        public async Task<IActionResult> OnPostAsync(int pageIndex, int? basarId)
        {
            await LoadBasarAsync(basarId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(BasarToEdit).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return RedirectToPage("/Basars/List", new { pageIndex });
        }

        public override IDictionary<string, string> GetRoute()
        {
            var route = base.GetRoute();
            route.Add("pageIndex", _pageIndex.ToString());
            return route;
        }
    }
}
