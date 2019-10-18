using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class CreateOrEdit : BasarPageModel
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CreateOrEdit(VeloBasarContext context, IStringLocalizer<SharedResource> localizer) : base(context)
        {
            _localizer = localizer;
        }

        [BindProperty]
        public Seller Seller { get; set; }
        public ListViewModel<Seller> Sellers { get; set; }
        [BindProperty]
        public bool IsValidationEnabled { get; set; }
        [BindProperty]
        public bool AreWeInEditMode { get; set; }
        public bool HasSellers { get => Sellers != null; }

        public async Task<IActionResult> OnGetAsync(int basarId, int? sellerId)
        {
            await Load(basarId);

            if (sellerId != null)
            {
                Seller = await Context.GetSellerAsync(sellerId.Value);
                if (Seller == null)
                {
                    return NotFound();
                }
                IsValidationEnabled = true;
                AreWeInEditMode = true;
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int basarId, bool? search, int? sellerId)
        {
            await Load(basarId);

            if (search == true)
            {
                var sellers = await Context.GetSellers(Seller.FirstName, Seller.LastName).ToListAsync();
                if (sellers.Count > 0)
                {
                    Sellers = new ListViewModel<Seller>(Basar, sellers, new[]{
                        new ListCommand<Seller>(GetItemRoute)
                        {
                            Page = "/Sellers/CreateOrEdit",
                            Text = _localizer["Übernehmen"]
                        }
                    });
                }
                return Page();
            }

            if (!ModelState.IsValid)
            {
                IsValidationEnabled = true;
                return Page();
            }

            if (sellerId != null)
            {
                Context.Attach(Seller).State = EntityState.Modified;
            }
            else
            {
                Context.Seller.Add(Seller);
            }

            await Context.SaveChangesAsync();
            return RedirectToPage("/Products/AddMany", new { basarId = Basar.Id, sellerId = Seller.Id });
        }
        public IDictionary<string, string> GetRoute(bool search)
        {
            var route = GetRoute();
            route.Add(nameof(search), search.ToString());
            return route;
        }
        public IDictionary<string, string> GetItemRoute(Seller seller)
        {
            var route = GetRoute();
            route.Add("sellerId", seller.Id.ToString());
            return route;
        }
        public IDictionary<string, string> GetPostRoute(bool? search = null)
        {
            var route = GetRoute();
            if(search != null)
            {
                route.Add(nameof(search), search.ToString());
            }
            else if (AreWeInEditMode)
            {
                route.Add("sellerId", Seller.Id.ToString());
            }
            return route;
        }

        private async Task Load(int basarId)
        {
            await LoadBasarAsync(basarId);
            ViewData["Countries"] = new SelectList(Context.Country, "Id", "Name");
        }
    }
}