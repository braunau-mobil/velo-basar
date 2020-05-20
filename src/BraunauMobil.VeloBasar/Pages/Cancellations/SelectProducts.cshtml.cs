using System;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class SelectProductsParameter
    {
        public int SaleId { get; set; }
    }
    [Authorize]
    public class SelectProductsModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public SelectProductsModel(IVeloContext context, ITransactionContext transactionContext)
        {
            _context = context;
            _transactionContext = transactionContext;
        }

        [BindProperty]
        public ListViewModel<Product> Products { get; set; }
        public string ErrorMessage { get; set; }
        public SelectProductsParameter Parameter { get; set; }

        public async Task OnGetAsync(SelectProductsParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            Parameter = parameter;

            var sale = await _transactionContext.GetAsync(parameter.SaleId);
            Products = new ListViewModel<Product>(_context.Basar, sale.Products.GetProducts());
        }
        public async Task<IActionResult> OnPostAsync(SelectProductsParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            Parameter = parameter;

            if (Products.List.All(vm => !vm.IsSelected))
            {
                ErrorMessage = _context.Localizer["Bitte ein Produkt zum Stornieren auswählen."];
                return Page();
            }

            var selectedProductIds = Products.List.Where(x => x.IsSelected).Select(x => x.Item.Id).ToList();
            var cancellation = await _transactionContext.CancelProductsAsync(_context.Basar, parameter.SaleId, selectedProductIds);
            if (await _transactionContext.ExistsAsync(parameter.SaleId))
            {
                return this.RedirectToPage<DoneModel>(new DoneParameter { CancellationId = cancellation.Id, SaleId = parameter.SaleId });
            }
            return this.RedirectToPage<DoneModel>(new DoneParameter { CancellationId = cancellation.Id });
        }
    }
}