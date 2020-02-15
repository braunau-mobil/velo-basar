using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BraunauMobil.VeloBasar.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

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
        private readonly IProductContext _productContext;
        private readonly ITransactionContext _transactionContext;

        public CartModel(IVeloContext context, IProductContext productContext, ITransactionContext transactionContext)
        {
            _context = context;
            _productContext = productContext;
            _transactionContext = transactionContext;
        }

        public string ErrorText { get; set; }
        public ProductsViewModel Products { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Artikel Id")]
        public int ProductId { get; set; }
        public int? SaleId { get; set; }

        public async Task OnGetAsync(CartParameter parameter)
        {
            Contract.Requires(parameter != null);

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
            Contract.Requires(parameter != null);

            var cart = Request.Cookies.GetCart();

            if (parameter.ProductId == null)
            {
                ErrorText = _context.Localizer["Bitte eine Product ID eingeben."];
            }
            else
            {
                var product = await _productContext.GetAsync(parameter.ProductId.Value);
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
        public VeloPage GetSalesDetailsPage() => this.GetPage<Transactions.DetailsModel>(new Transactions.DetailsParameter { TransactionId = SaleId.Value });
        
        private async Task LoadProducts(IList<int> productIds)
        {
            Products = await ProductsViewModel.CreateAsync(_productContext.GetMany(productIds),
            async vm =>
            {
                if (!vm.Product.IsAllowed(TransactionType.Sale))
                {
                    vm.HasAlert = true;
                    vm.Alert = await GetProductErrorAsync(vm.Product, vm.Product.Id);
                }
            },
            new[]
            {
                new ListCommand<ProductViewModel>(vm => this.GetPage<CartModel>(new CartParameter { Delete = true, ProductId = vm.Product.Id }))
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
                var transaction = await _transactionContext.GetLatestAsync(_context.Basar, product.Id);
                return _context.Localizer["Der Artikel wurde als verschwunden markiert. Anmerkungen: {0}", transaction.Notes];
            }
            else if (product.StorageState == StorageState.Locked)
            {
                var transaction = await _transactionContext.GetLatestAsync(_context.Basar, product.Id);
                return _context.Localizer["Der Artikel wurde gesperrt. Anmerkungen: {0}", transaction.Notes];
            }
            else if (product.StorageState == StorageState.Sold)
            {
                var sale = await _transactionContext.GetLatestAsync(_context.Basar, product.Id);
                SaleId = sale.Id;
                return _context.Localizer["Der Artikel wurde bereits verkauft. Siehe Verkauf #{0}", sale.Number];
            }
            return null;
        }
    }
}