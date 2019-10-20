﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class SelectProductsModel : BasarPageModel
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private int _saleId;

        public SelectProductsModel(VeloBasarContext context, IStringLocalizer<SharedResource> localizer) : base(context)
        {
            _localizer = localizer;
        }

        [BindProperty]
        public ListViewModel<Product> Products { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(int basarId, int saleId)
        {
            await LoadBasarAsync(basarId);
            _saleId = saleId;

            var sale = await Context.Transactions.GetAsync(saleId);
            Products = new ListViewModel<Product>(Basar, sale.Products.GetProducts());
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int saleId)
        {
            await LoadBasarAsync(basarId);

            if (Products.List.All(vm => !vm.IsSelected))
            {
                ErrorMessage = _localizer["Bitte ein Produkt zum Stornieren auswählen."];
                return Page();
            }

            var cancellation = await Context.CancelProductsAsync(Basar, saleId, Products.List.Where(vm => vm.IsSelected).Select(vm => vm.Item).ToArray());
            if (await Context.Transactions.ExistsAsync(saleId))
            {
                return RedirectToPage("/Cancellations/Done", new { basarId, cancellationId = cancellation.Id, saleId });
            }
            return RedirectToPage("/Cancellations/Done", new { basarId, cancellationId = cancellation.Id });

        }

        public override IDictionary<string, string> GetRoute()
        {
            var route = base.GetRoute();
            route.Add("saleId", _saleId.ToString());
            return route;
        }
    }
}