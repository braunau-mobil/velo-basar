using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        private readonly IBrandContext _brandContext;
        private readonly IProductTypeContext _productTypeContext;
        private EnterProductsParameter _parameter;

        public EnterProductsModel(IVeloContext context, IBrandContext brandContext, IProductTypeContext productTypeContext)
        {
            _context = context;
            _brandContext = brandContext;
            _productTypeContext = productTypeContext;
        }

        public bool AreWeInEditMode { get; set; }
        [BindProperty]
        public Product NewProduct { get; set; }
        public EnterProductsParameter Parameter { get; set; }
        public ListViewModel<Product> Products { get; set; }
        public int SellerId { get => _parameter.SellerId; }

        public void OnGet(EnterProductsParameter parameter)
        {
            Contract.Requires(parameter != null);

            _parameter = parameter;
            CreateSelectLists();

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
            Contract.Requires(parameter != null);

            _parameter = parameter;

            CreateSelectLists();

            var products = Request.Cookies.GetAcceptanceProducts();
            
            if (ModelState.IsValid)
            {
                NewProduct.Basar = _context.Basar;
                NewProduct.Brand = await _brandContext.GetAsync(NewProduct.BrandId);
                NewProduct.Type = await _productTypeContext.GetAsync(NewProduct.TypeId);
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

        private void CreateSelectLists()
        {
            ViewData["Brands"] = _brandContext.GetSelectList();
            ViewData["ProductTypes"] = _productTypeContext.GetSelectList();
        }
        private ListViewModel<Product> CreateViewModels(IList<Product> products)
        {
            return new ListViewModel<Product>(_context.Basar, products.ToArray(), new[]
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