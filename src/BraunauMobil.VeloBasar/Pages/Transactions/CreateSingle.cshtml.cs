using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Transactions
{
    public class CreateSingleModel : BasarPageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public CreateSingleModel(VeloBasarContext context, SignInManager<IdentityUser> signInManager) : base(context)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        [Display(Name = "Anmerkungen")]
        public string Notes { get; set; }
        public Product Product { get; set; }
        public TransactionType TransactionType { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; }

        public async Task<IActionResult> OnGetAsync(int basarId, int productId, TransactionType transactionType)
        {
            if (transactionType == TransactionType.Release && !_signInManager.IsSignedIn(User))
            {
                return Forbid();
            }

            await LoadBasarAsync(basarId);
            TransactionType = transactionType;

            Product = await Context.GetProductAsync(productId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int basarId, int productId, TransactionType transactionType)
        {
            await LoadBasarAsync(basarId);

            Product = await Context.Product.GetAsync(productId);
            await Context.DoTransactionAsync(Basar, transactionType, Notes, Product);

            return RedirectToPage("/Products/Details", new { basarId, productId });
        }
        public IDictionary<string, string> GetPostRoute()
        {
            var route = GetRoute();
            route.Add("productId", Product.Id.ToString());
            route.Add("transactionType", TransactionType.ToString());
            return route;
        }
    }
}