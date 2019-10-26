using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class EnterProductsModel : BasarPageModel
    {
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private int _sellerId;

        public EnterProductsModel(VeloBasarContext context, IStringLocalizer<SharedResource> localizer, IHtmlGenerator htmlGenerator) : base(context)
        {
            _htmlGenerator = htmlGenerator;
            _localizer = localizer;
        }

        public bool AreWeInEditMode { get; set; }
        [BindProperty]
        public Product NewProduct { get; set; }
        public ListViewModel<Product> Products { get; set; }

        public async Task OnGetAsync(int basarId, int sellerId, int? productId, bool? delete)
        {
            await LoadBasarAsync(basarId);
            _sellerId = sellerId;
            ViewData["Brands"] = new SelectList(Context.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(Context.ProductTypes, "Id", "Name");

            var products = Request.Cookies.GetAcceptanceProducts();
            
            if (productId != null)
            {
                if (delete == true)
                {
                    products.RemoveAt(productId.Value);
                    for (var index = 0; index < products.Count; index++)
                    {
                        products[index].Id = index;
                    }
                }
                else
                {
                    NewProduct = products[productId.Value];
                    AreWeInEditMode = true;
                }
            }

            Response.Cookies.SetAcceptanceProducts(products);

            Products = CreateViewModels(products);
        }
        public async Task<IActionResult> OnPostAsync(int basarId, int sellerId, int? productId)
        {
            await LoadBasarAsync(basarId);
            _sellerId = sellerId;
            ViewData["Brands"] = new SelectList(Context.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(Context.ProductTypes, "Id", "Name");

            var products = Request.Cookies.GetAcceptanceProducts();
            
            if (ModelState.IsValid)
            {
                NewProduct.Brand = await Context.Brand.GetAsync(NewProduct.BrandId);
                NewProduct.Type = await Context.ProductTypes.GetAsync(NewProduct.TypeId);
                if (productId != null)
                {
                    products[NewProduct.Id] = NewProduct;
                }
                else
                {
                    NewProduct.Id = products.Count();
                    products.Add(NewProduct);
                }

                NewProduct = new Product();
                ModelState.Clear();
            }

            Response.Cookies.SetAcceptanceProducts(products);
            Products = CreateViewModels(products);

            return Page();
        }
        public IDictionary<string, string> GetAcceptRoute()
        {
            var route = GetRoute();
            route.Add("sellerId", _sellerId.ToString());
            return route;
        }
        public IDictionary<string, string> GetAddRoute()
        {
            var route = GetRoute();
            route.Add("sellerId", _sellerId.ToString());
            return route;
        }
        public IDictionary<string, string> GetCancelRoute()
        {
            var route = GetRoute();
            route.Add("sellerId", _sellerId.ToString());
            return route;
        }
        public IDictionary<string, string> GetUpdateRoute()
        {
            var route = GetRoute();
            route.Add("sellerId", _sellerId.ToString());
            route.Add("productId", NewProduct.Id.ToString());
            return route;
        }

        private ListViewModel<Product> CreateViewModels(IList<Product> products)
        {
            return new ListViewModel<Product>(Basar, products, new[]
            {
                new ListCommand<Product>(GetEditItemRoute)
                {
                    Page = Request.Path,
                    Text = _localizer["Editieren"]
                },
                new ListCommand<Product>(GetDeleteItemRoute)
                {
                    Page = Request.Path,
                    Text = _localizer["Löschen"]
                }
            });
        }
        private IDictionary<string, string> GetDeleteItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("sellerId", _sellerId.ToString());
            route.Add("productId", product.Id.ToString());
            route.Add("delete", true.ToString());
            return route;
        }
        private IDictionary<string, string> GetEditItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("sellerId", _sellerId.ToString());
            route.Add("productId", product.Id.ToString());
            return route;
        }
    }
}