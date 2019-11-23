using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Transactions
{
    public class CreateSingleParameter
    {
        public int ProductId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
    [Authorize]
    public class CreateSingleModel : PageModel
    {
        private readonly IVeloContext _context;

        public CreateSingleModel(IVeloContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Display(Name = "Anmerkungen")]
        public string Notes { get; set; }
        public Product Product { get; set; }
        public TransactionType TransactionType { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; }

        public async Task<IActionResult> OnGetAsync(CreateSingleParameter parameter)
        {
            if (parameter.TransactionType == TransactionType.Release && !_context.SignInManager.IsSignedIn(User))
            {
                return Forbid();
            }

            TransactionType = parameter.TransactionType;

            Product = await _context.Db.Product.GetAsync(parameter.ProductId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CreateSingleParameter parameter)
        {
            Product = await _context.Db.Product.GetAsync(parameter.ProductId);
            var printSettings = await _context.Db.GetPrintSettingsAsync();
            await _context.Db.DoTransactionAsync(_context.Basar, parameter.TransactionType, Notes, printSettings, Product);

            return this.RedirectToPage<Products.DetailsModel>(new Products.DetailsParameter { ProductId = parameter.ProductId });
        }
        public object GetPostParameter() => new CreateSingleParameter { ProductId = Product.Id, TransactionType = TransactionType };
    }
}