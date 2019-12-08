using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Logic;
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
        private readonly ITransactionContext _transactionContext;
        private readonly IProductContext _productContext;

        public CreateSingleModel(IVeloContext context, ITransactionContext transactionContext, IProductContext productContext)
        {
            _context = context;
            _transactionContext = transactionContext;
            _productContext = productContext;
        }

        [BindProperty]
        [Display(Name = "Anmerkungen")]
        public string Notes { get; set; }
        public Product Product { get; set; }
        public TransactionType TransactionType { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; }

        public async Task<IActionResult> OnGetAsync(CreateSingleParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (parameter.TransactionType == TransactionType.Release && !_context.SignInManager.IsSignedIn(User))
            {
                return Forbid();
            }

            TransactionType = parameter.TransactionType;

            Product = await _productContext.GetAsync(parameter.ProductId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CreateSingleParameter parameter)
        {
            Contract.Requires(parameter != null);

            await _transactionContext.DoTransactionAsync(_context.Basar, parameter.TransactionType, Notes, parameter.ProductId);

            return this.RedirectToPage<Products.DetailsModel>(new Products.DetailsParameter { ProductId = parameter.ProductId });
        }
        public object GetPostParameter() => new CreateSingleParameter { ProductId = Product.Id, TransactionType = TransactionType };
    }
}