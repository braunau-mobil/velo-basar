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

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class StartWithNewSeller : BasarPageModel
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StartWithNewSeller(VeloBasarContext context, IStringLocalizer<SharedResource> localizer) : base(context)
        {
            _localizer = localizer;
        }

        public string ErrorText { get; set; }
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
                Seller = await Context.Seller.GetAsync(sellerId.Value);
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
                if (string.IsNullOrEmpty(Seller.FirstName) && string.IsNullOrEmpty(Seller.LastName))
                {
                    ErrorText = _localizer["Bitte Vor und/oder Nachnamen eingeben für die Suche"];
                    return Page();
                }

                var sellers = await Context.Seller.GetMany(Seller.FirstName, Seller.LastName).ToListAsync();
                if (sellers.Count > 0)
                {
                    Sellers = new ListViewModel<Seller>(Basar, sellers, new[]{
                        new ListCommand<Seller>(GetItemRoute)
                        {
                            Page = Request.Path,
                            Text = _localizer["Übernehmen"]
                        }
                    });
                }
                else
                {
                    ErrorText = _localizer["Es konnte kein Verkäufer gefunden werden."];
                }
                return Page();
            }

            if (string.IsNullOrEmpty(Seller.FirstName) || string.IsNullOrEmpty(Seller.LastName))
            {
                IsValidationEnabled = true;
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
            return RedirectToPage("/Acceptances/EnterProducts", new { basarId = Basar.Id, sellerId = Seller.Id });
        }
        public IDictionary<string, string> GetItemRoute(Seller seller)
        {
            var route = GetRoute();
            route.Add("sellerId", seller.Id.ToString());
            return route;
        }
        public IDictionary<string, string> GetNextRoute()
        {
            var route = GetRoute();
            if (AreWeInEditMode)
            {
                route.Add("sellerId", Seller.Id.ToString());
            }
            return route;
        }
        public IDictionary<string, string> GetSearchRoute()
        {
            var route = GetRoute();
            route.Add("search", true.ToString());
            return route;
        }

        private async Task Load(int basarId)
        {
            await LoadBasarAsync(basarId);
            ViewData["Countries"] = new SelectList(Context.Country, "Id", "Name");
        }
    }
}