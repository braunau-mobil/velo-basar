using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class EnterProductsParameter
    {
        public int SellerId { get; set; }
        public int? ProductId { get; set; }
        public bool? Delete { get; set; }
    }

    public class EnterProductsModel : PageModel
    {
        private readonly IVeloContext _context;
        private EnterProductsParameter _parameter;

        public EnterProductsModel(IVeloContext context)
        {
            _context = context;
        }

        public bool AreWeInEditMode { get; set; }
        [BindProperty]
        public Product NewProduct { get; set; }
        public EnterProductsParameter Parameter { get; set; }
        public ListViewModel<Product> Products { get; set; }

        public void OnGet(EnterProductsParameter parameter)
        {
            _parameter = parameter;

            ViewData["Brands"] = new SelectList(_context.Db.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(_context.Db.ProductTypes, "Id", "Name");

            var products = Request.Cookies.GetAcceptanceProducts();
            
            if (parameter.ProductId != null)
            {
                if (parameter.Delete == true)
                {
                    products.RemoveAt(parameter.ProductId.Value);
                    for (var index = 0; index < products.Count; index++)
                    {
                        products[index].Id = index;
                    }
                }
                else
                {
                    NewProduct = products[parameter.ProductId.Value];
                    AreWeInEditMode = true;
                }
            }

            Response.Cookies.SetAcceptanceProducts(products);

            Products = CreateViewModels(products);
        }
        public async Task<IActionResult> OnPostAsync(EnterProductsParameter parameter)
        {
            _parameter = parameter;

            ViewData["Brands"] = new SelectList(_context.Db.Brand, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(_context.Db.ProductTypes, "Id", "Name");

            var products = Request.Cookies.GetAcceptanceProducts();
            
            if (ModelState.IsValid)
            {
                NewProduct.Brand = await _context.Db.Brand.GetAsync(NewProduct.BrandId);
                NewProduct.Type = await _context.Db.ProductTypes.GetAsync(NewProduct.TypeId);
                if (parameter.ProductId != null)
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
        public VeloPage GetAcceptPage() => this.GetPage<AcceptModel>(new AcceptParameter { SellerId = _parameter.SellerId });
        public object GetAddParameter() => new EnterProductsParameter { SellerId = _parameter.SellerId };
        public VeloPage GetCancelPage() => this.GetPage<Sellers.DetailsModel>(new Sellers.DetailsParameter { SellerId = _parameter.SellerId });
        public object GetUpdateParameter() => new EnterProductsParameter { ProductId = NewProduct.Id, SellerId = _parameter.SellerId };

        private ListViewModel<Product> CreateViewModels(IList<Product> products)
        {
            return new ListViewModel<Product>(_context.Basar, products, new[]
            {
                new ListCommand<Product>(product => this.GetPage<EnterProductsModel>(new EnterProductsParameter { ProductId = product.Id, SellerId = _parameter.SellerId }))
                {
                    Text = _context.Localizer["Editieren"]
                },
                new ListCommand<Product>(product => this.GetPage<EnterProductsModel>(new EnterProductsParameter { Delete = true, ProductId = product.Id, SellerId = _parameter.SellerId }))
                {
                    Text = _context.Localizer["Löschen"]
                }
            });
        }
    }
}