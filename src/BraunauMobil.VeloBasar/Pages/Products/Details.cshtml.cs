using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.ViewModels;
using System;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class DetailsParameter
    {
        public int ProductId { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly IProductContext _productContext;
        private readonly ITransactionContext _transactionContext;

        public DetailsModel(IVeloContext context, IProductContext productContext, ITransactionContext transactionContext)
        {
            _context = context;
            _productContext = productContext;
            _transactionContext = transactionContext;
        }

        [BindProperty]
        public Product Product { get; set; }

        public TransactionsViewModel Transactions { get; set; }

        public async Task<IActionResult> OnGetAsync(DetailsParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            Product = await _productContext.GetAsync(parameter.ProductId);

            if (Product == null)
            {
                return NotFound();
            }

            Transactions = await TransactionsViewModel.CreateAsync(_transactionContext.GetFromProduct(_context.Basar, Product.Id));
            Transactions.ShowType = true;
            Transactions.ShowDocumentLink = true;
            Transactions.ShowNotes = true;

            return Page();
        }
        public VeloPage GetEditPage() => this.GetPage<EditModel>(new EditParameter { ProductId = Product.Id });
        public VeloPage GetPage(TransactionType transactionType) => this.GetPage<Transactions.CreateSingleModel>(new Transactions.CreateSingleParameter { ProductId = Product.Id, TransactionType = transactionType });
        public VeloPage GetShowFilePage() => this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = Product.LabelId });
        public VeloPage GetSellerDetailsPage()
        {
            return new VeloPage
            {
                Page = RoutingHelper.GetPageForModel<Sellers.DetailsModel>(),
                Parameter = new Pages.Sellers.DetailsParameter { SellerId = Product.Seller.Id }
            };
        }
    }
}
