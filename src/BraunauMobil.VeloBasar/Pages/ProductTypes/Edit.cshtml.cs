using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class EditModel : BasarPageModel
    {
        private int _pageIndex;

        public EditModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public ProductType ProductType { get; set; }

        public async Task<IActionResult> OnGetAsync(int ProductTypeId, int pageIndex, int? basarId)
        {
            await LoadBasarAsync(basarId);
            ProductType = await Context.GetProductTypeAsync(ProductTypeId);
            _pageIndex = pageIndex;

            if (ProductType == null)
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

            Context.Attach(ProductType).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return RedirectToPage("/ProductTypes/List", new { pageIndex });
        }

        public override IDictionary<string, string> GetRoute()
        {
            var route = base.GetRoute();
            route.Add("pageIndex", _pageIndex.ToString());
            return route;
        }
    }
}
