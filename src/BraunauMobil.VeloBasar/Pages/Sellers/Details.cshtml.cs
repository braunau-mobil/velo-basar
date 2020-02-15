using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;
using BraunauMobil.VeloBasar.ViewModels;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class DetailsParameter
    {
        public int SellerId { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly IStatisticContext _statisticContext;
        private readonly ISellerContext _sellerContext;
        private readonly IProductContext _productContext;
        private readonly ITransactionContext _transactionContext;

        public DetailsModel(IVeloContext context, ISellerContext sellerContext, IStatisticContext statisticContext, IProductContext productContext, ITransactionContext transactionContext)
        {
            _context = context;
            _sellerContext = sellerContext;
            _statisticContext = statisticContext;
            _productContext = productContext;
            _transactionContext = transactionContext;
        }

        public TransactionsViewModel Acceptances { get; set; }
        public TransactionsViewModel Settlements { get; set; }
        public Seller Seller { get; set; }
        public SellerStatistics Stats { get; set; }
        public ProductsViewModel Products { get; set; }

        public async Task<IActionResult> OnGetAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            Seller = await _sellerContext.GetAsync(parameter.SellerId);
            if (Seller == null)
            {
                return NotFound();
            }

            Acceptances = await TransactionsViewModel.CreateAsync(_transactionContext.GetMany(_context.Basar, TransactionType.Acceptance, parameter.SellerId), null, new[]
            {
                new ListCommand<TransactionItemViewModel>(GetTransactionDetailsPage)
                {
                    Text  = _context.Localizer["Details"]
                }
            });
            Acceptances.ShowDocumentLink = true;
            Settlements = await TransactionsViewModel.CreateAsync(_transactionContext.GetMany(_context.Basar, TransactionType.Settlement, parameter.SellerId), null, new[]
            {
                new ListCommand<TransactionItemViewModel>(GetTransactionDetailsPage)
                {
                    Text  = _context.Localizer["Details"]
                },
                new ListCommand<TransactionItemViewModel>(tx => _context.SignInManager.IsSignedIn(User), GetRevertSettlementPage)
                {
                    Text = _context.Localizer["Zurücksetzen"]
                }
            });
            Settlements.ShowDocumentLink = true;
            Products = await ProductsViewModel.CreateAsync(_productContext.GetProductsForSeller(_context.Basar, parameter.SellerId));
            Stats = await _statisticContext.GetSellerStatisticsAsync(_context.Basar, Seller.Id);

            return Page();
        }
        public VeloPage GetPrintLablesPage() => this.GetPage<Labels.PrintForSellerModel>(new Labels.PrintForSellerParameter { SellerId = Seller.Id });
        public VeloPage GetCreateSettlementPage() => this.GetPage<Settlements.CreateAndPrintModel>(new Settlements.CreateAndPrintParameter { SellerId = Seller.Id });
        public VeloPage GetEditPage() => this.GetPage<EditModel>(new EditParameter { SellerId = Seller.Id });
        public VeloPage GetStartAcceptancePage() => this.GetPage<Acceptances.StartWithSellerModel>(new Acceptances.StartWithSellerParameter { SellerId = Seller.Id });
        private VeloPage GetRevertSettlementPage(TransactionItemViewModel viewModel) => this.GetPage<Transactions.RevertModel>(new Transactions.RevertParameter { TransactionId = viewModel.Transaction.Id });
        private VeloPage GetTransactionDetailsPage(TransactionItemViewModel viewModel) => this.GetPage<Transactions.DetailsModel>(new Transactions.DetailsParameter { TransactionId = viewModel.Transaction.Id });
    }
}
