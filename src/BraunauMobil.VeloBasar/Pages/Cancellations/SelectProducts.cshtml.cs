using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class SelectProductsParameter
    {
        public int SaleId { get; set; }
    }
    public class SelectProductsModel : PageModel
    {
        private readonly IVeloContext _context;

        public SelectProductsModel(IVeloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ListViewModel<Product> Products { get; set; }
        public string ErrorMessage { get; set; }
        public SelectProductsParameter Parameter { get; set; }

        public async Task OnGetAsync(SelectProductsParameter parameter)
        {
            Parameter = parameter;

            var sale = await _context.Db.Transactions.GetAsync(parameter.SaleId);
            Products = new ListViewModel<Product>(_context.Basar, sale.Products.GetProducts());
        }
        public async Task<IActionResult> OnPostAsync(SelectProductsParameter parameter)
        {
            Parameter = parameter;

            if (Products.List.All(vm => !vm.IsSelected))
            {
                ErrorMessage = _context.Localizer["Bitte ein Produkt zum Stornieren auswählen."];
                return Page();
            }

            var cancellation = await _context.Db.CancelProductsAsync(_context.Basar, parameter.SaleId, Products.List.Where(vm => vm.IsSelected).Select(vm => vm.Item).ToArray());
            if (await _context.Db.Transactions.ExistsAsync(parameter.SaleId))
            {
                return this.RedirectToPage<DoneModel>(new DoneParameter { CancellationId = cancellation.Id, SaleId = parameter.SaleId });
            }
            return this.RedirectToPage<DoneModel>(new DoneParameter { CancellationId = cancellation.Id });
        }
    }
}