using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class EditModel : BasarPageModel
    {
        private int _pageIndex;

        public EditModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Brand Brand { get; set; }

        public async Task<IActionResult> OnGetAsync(int brandId, int pageIndex)
        {
            Brand = await Context.GetBrandAsync(brandId);
            _pageIndex = pageIndex;

            if (Brand == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int pageIndex)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(Brand).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return RedirectToPage("/Brands/List", new { pageIndex });
        }

        public IDictionary<string, string> GetListRoute()
        {
            var route = new Dictionary<string, string>();
            route.Add("pageIndex", _pageIndex.ToString());
            return route;
        }
    }
}
