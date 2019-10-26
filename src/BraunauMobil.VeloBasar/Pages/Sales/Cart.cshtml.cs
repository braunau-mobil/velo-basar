using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;
using BraunauMobil.VeloBasar.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CartModel : BasarPageModel
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IHtmlHelper<CartModel> _htmlHelper;

        public CartModel(VeloBasarContext context, IStringLocalizer<SharedResource> localizer, IHtmlHelper<CartModel> htmlHelper) : base(context)
        {
            _localizer = localizer;
            _htmlHelper = htmlHelper;
        }

        public string ErrorText { get; set; }

        public ListViewModel<Product> Products { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Artikel Id")]
        public int ProductId { get; set; }

        public async Task OnGetAsync(int basarId, bool? showError = null, int? productId = null, bool? delete = null)
        {
            await LoadBasarAsync(basarId);

            var productIds = Request.Cookies.GetCart();
            if (productId != null && delete != null && delete == true)
            {
                productIds.Remove(productId.Value);
                Response.Cookies.SetCart(productIds);
            }

            if (showError != null)
            {
                ErrorText = _localizer["Ein oder mehrere Artikel konnten nicht verkauft werden."];
            }

            await LoadProducts(productIds);
        }
        public async Task<IActionResult> OnPostAsync(int basarId, int productId)
        {
            await LoadBasarAsync(basarId);

            var cart = Request.Cookies.GetCart();
            var product = await Context.Product.GetAsync(productId);
            if (product != null && product.IsAllowed(TransactionType.Sale))
            {
                cart.Add(productId);
                Response.Cookies.SetCart(cart);
            }

            ErrorText = await GetProductErrorAsync(product, productId);

            await LoadProducts(cart);

            return Page();
        }
        public IDictionary<string, string> GetDeleteItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("productId", product.Id.ToString());
            route.Add("delete", true.ToString());
            return route;
        }
        private async Task LoadProducts(IList<int> productIds)
        {
            var viewModels = new List<ItemViewModel<Product>>();
            foreach (var product in await Context.Product.GetMany(productIds).ToArrayAsync())
            {
                var viewModel = new ItemViewModel<Product>
                {
                    Item = product
                };
                if (!product.IsAllowed(TransactionType.Sale))
                {
                    viewModel.HasAlert = true;
                    viewModel.Alert = await GetProductErrorAsync(product, product.Id);
                }
                viewModels.Add(viewModel);
            }

            Products = new ListViewModel<Product>(Basar, viewModels, new[]
            {
                new ListCommand<Product>(GetDeleteItemRoute)
                {
                    Page = Request.Path,
                    Text = _localizer["Löschen"]
                }
            });
        }
        private async Task<string> GetProductErrorAsync(Product product, int productId)
        {
            if (product == null)
            {
                return _localizer["Es konnte kein Artikel mit der ID {0} gefunden werden.", productId];
            }
            else if (product.ValueStatus == ValueStatus.Settled)
            {
                return _localizer["Der Artikel wurde bereits abgerechnet."];
            }
            else if (product.StorageState == StorageState.Gone)
            {
                //  @todo Letzte TX anzeigen!
                return _localizer["Der Artikel wurde als verschwunden markiert. Anmerkungen: @todo"];
            }
            else if (product.StorageState == StorageState.Locked)
            {
                //  @todo Letzte TX anzeigen!
                return _localizer["Der Artikel wurde gesperrt. Anmerkungen: @todo"];
            }
            else if (product.StorageState == StorageState.Sold)
            {
                var saleNumber = await Context.GetTransactionNumberForProductAsync(Basar, TransactionType.Sale, product.Id);

                //  @todo generate link to sale details with blank target
                return _localizer["Der Artikel wurde bereits verkauft. Siehe Verkauf #{0}", saleNumber];
            }
            return null;
        }
    }
}