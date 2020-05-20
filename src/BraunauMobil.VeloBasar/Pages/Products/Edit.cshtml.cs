using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class EditParameter
    {
        public int ProductId { get; set; }
        public string Target { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly IProductContext _productContext;
        private readonly IBrandContext _brandContext;
        private readonly IProductTypeContext _productTypeContext;
        private readonly ITransactionContext _transactionContext;

        public EditModel(IProductContext productContext , IBrandContext brandContext, IProductTypeContext productTypeContext, ITransactionContext transactionContext)
        {
            _productContext = productContext;
            _brandContext = brandContext;
            _productTypeContext = productTypeContext;
            _transactionContext = transactionContext;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            UpdateSelectionLists();

            Product = await _productContext.GetAsync(parameter.ProductId);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            UpdateSelectionLists();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _transactionContext.UpdateProductAsync(Product);

            return Redirect(parameter.Target);
        }

        private void UpdateSelectionLists()
        {
            ViewData["Brands"] = _brandContext.GetSelectList();
            ViewData["ProductTypes"] = _productTypeContext.GetSelectList();
        }
    }
}
