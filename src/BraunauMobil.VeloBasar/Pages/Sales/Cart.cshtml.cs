using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BraunauMobil.VeloBasar.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CartParameter
    {
        public bool? ShowError { get; set; }
        public int? ProductId { get; set; }
        public bool? Delete { get; set; }
    }
    public class CartModel : PageModel
    {
        private readonly IVeloContext _context;

        public CartModel(IVeloContext context)
        {
            _context = context;
        }

        public string ErrorText { get; set; }

        public ListViewModel<Product> Products { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Artikel Id")]
        public int ProductId { get; set; }

        public async Task OnGetAsync(CartParameter parameter)
        {
            var productIds = Request.Cookies.GetCart();
            if (parameter.ProductId != null && parameter.Delete != null && parameter.Delete == true)
            {
                productIds.Remove(parameter.ProductId.Value);
                Response.Cookies.SetCart(productIds);
            }

            if (parameter.ShowError != null)
            {
                ErrorText = _context.Localizer["Ein oder mehrere Artikel konnten nicht verkauft werden."];
            }

            await LoadProducts(productIds);
        }
        public async Task OnPostAsync(CartParameter parameter)
        {
            var cart = Request.Cookies.GetCart();

            if (parameter.ProductId == null)
            {
                ErrorText = _context.Localizer["Bitte eine Product ID eingeben."];
            }
            else
            {
                var product = await _context.Db.Product.GetAsync(parameter.ProductId.Value);
                if (product != null)
                {
                    ErrorText = await GetProductErrorAsync(product, parameter.ProductId.Value);
                    if (product.IsAllowed(TransactionType.Sale))
                    {
                        cart.Add(parameter.ProductId.Value);
                    }
                }
            }
            
            Response.Cookies.SetCart(cart);

            await LoadProducts(cart);
        }
        
        private async Task LoadProducts(IList<int> productIds)
        {
            var viewModels = new List<ItemViewModel<Product>>();
            foreach (var product in await _context.Db.Product.GetMany(productIds).ToArrayAsync())
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

            Products = new ListViewModel<Product>(_context.Basar, viewModels, new[]
            {
                new ListCommand<Product>(product => this.GetPage<CartModel>(new CartParameter { Delete = true, ProductId = product.Id }))
                {
                    Text = _context.Localizer["Löschen"]
                }
            });
        }
        private async Task<string> GetProductErrorAsync(Product product, int productId)
        {
            if (product == null)
            {
                return _context.Localizer["Es konnte kein Artikel mit der ID {0} gefunden werden.", productId];
            }
            else if (product.ValueState == ValueState.Settled)
            {
                return _context.Localizer["Der Artikel wurde bereits abgerechnet."];
            }
            else if (product.StorageState == StorageState.Gone)
            {
                //  @todo Letzte TX anzeigen!
                return _context.Localizer["Der Artikel wurde als verschwunden markiert. Anmerkungen: @todo"];
            }
            else if (product.StorageState == StorageState.Locked)
            {
                //  @todo Letzte TX anzeigen!
                return _context.Localizer["Der Artikel wurde gesperrt. Anmerkungen: @todo"];
            }
            else if (product.StorageState == StorageState.Sold)
            {
                var saleNumber = await _context.Db.GetTransactionNumberForProductAsync(_context.Basar, TransactionType.Sale, product.Id);

                //  @todo generate link to sale details with blank target
                return _context.Localizer["Der Artikel wurde bereits verkauft. Siehe Verkauf #{0}", saleNumber];
            }
            return null;
        }
    }
}