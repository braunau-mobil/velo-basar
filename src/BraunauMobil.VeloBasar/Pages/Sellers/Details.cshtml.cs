using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

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

        public DetailsModel(IVeloContext context, ISellerContext sellerContext, IStatisticContext statisticContext, IProductContext productContext)
        {
            _context = context;
            _sellerContext = sellerContext;
            _statisticContext = statisticContext;
            _productContext = productContext;
        }

        public IReadOnlyList<TransactionStatistic> AcceptanceStatistics { get; set; }
        public IReadOnlyList<TransactionStatistic> SettlementStatistics { get; set; }
        public bool CanSettle { get; set; }
        public Seller Seller { get; set; }
        public SellerStatistics Stats { get; set; }
        public IReadOnlyList<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            Seller = await _sellerContext.GetAsync(parameter.SellerId);
            if (Seller == null)
            {
                return NotFound();
            }

            AcceptanceStatistics = await _statisticContext.GetTransactionStatistics(TransactionType.Acceptance, _context.Basar, parameter.SellerId);
            SettlementStatistics = await _statisticContext.GetTransactionStatistics(TransactionType.Settlement, _context.Basar, parameter.SellerId);
            Products = await _productContext.GetProductsForSeller(_context.Basar, parameter.SellerId).AsNoTracking().ToListAsync();
            Stats = await _statisticContext.GetSellerStatisticsAsync(_context.Basar, Seller.Id);

            return Page();
        }
        public VeloPage GetAcceptanceDetailsPage(TransactionStatistic item)
        {
            Contract.Requires(item != null);
            return this.GetPage<Acceptances.DetailsModel>(new Acceptances.DetailsParameter { AcceptanceId = item.Transaction.Id });
        }
        public VeloPage GetCreateLabelsPage() => this.GetPage<Labels.CreateAndPrintForSellerModel>(new Labels.CreateAndPrintForSellerParameter { SellerId = Seller.Id });
        public VeloPage GetCreateSettlementPage() => this.GetPage<Settlements.CreateAndPrintModel>(new Settlements.CreateAndPrintParameter { SellerId = Seller.Id });
        public VeloPage GetDocumentPage(ProductsTransaction transaction)
        {
            Contract.Requires(transaction != null);
            return this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = transaction.DocumentId.Value });
        }
        public VeloPage GetEditPage() => this.GetPage<EditModel>(new EditParameter { SellerId = Seller.Id });
        public VeloPage GetStartAcceptancePage() => this.GetPage<Acceptances.StartWithSellerModel>(new Acceptances.StartWithSellerParameter { SellerId = Seller.Id });
    }
}
