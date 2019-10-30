using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class DetailsParameter
    {
        public int ProductId { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public DetailsModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(DetailsParameter parameter)
        {
            Product = await _context.Product.GetAsync(parameter.ProductId);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
        public VeloPage GetEditPage() => this.GetPage<EditModel>(new EditParameter { ProductId = Product.Id });
        public VeloPage GetPage(TransactionType transactionType) => this.GetPage<Transactions.CreateSingleModel>(new Transactions.CreateSingleParameter { ProductId = Product.Id, TransactionType = transactionType });
        public VeloPage GetShowFilePage() => this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = Product.Label.Value });
    }
}
