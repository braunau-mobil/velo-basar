using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class PrintForSellerParameter
    {
        public int SellerId { get; set; }
    }
    public class PrintForSellerModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly IProductContext _productContext;
        private readonly IFileStoreContext _fileStoreContext;

        public PrintForSellerModel(IVeloContext context, IProductContext productContext, IFileStoreContext fileStoreContext)
        {
            _context = context;
            _productContext = productContext;
            _fileStoreContext = fileStoreContext;
        }

        public async Task<IActionResult> OnGetAsync(PrintForSellerParameter parameter)
        {
            Contract.Requires(parameter != null);

            var products = await _productContext.GetProductsForSeller(_context.Basar, parameter.SellerId).ToArrayAsync();
            var file = await _fileStoreContext.GetProductLabelsAndCombineToOnePdfAsync(products);

            return File(file.Data, file.ContentType);
        }
    }
}