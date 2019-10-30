using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class DetailsParameter
    {
        public int SellerId { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly IVeloContext _context;

        public DetailsModel(IVeloContext context)
        {
            _context = context;
        }

        public IList<TransactionStatistic> AcceptanceStatistics { get; set; }
        public IList<TransactionStatistic> SettlementStatistics { get; set; }
        public bool CanSettle { get; set; }
        public Seller Seller { get; set; }
        public SellerStatistics Stats { get; set; }
        public IList<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(DetailsParameter parameter)
        {
            Seller = await _context.Db.Seller.Include(s => s.Country).FirstOrDefaultAsync(m => m.Id == parameter.SellerId);
            if (Seller == null)
            {
                return NotFound();
            }

            AcceptanceStatistics = await _context.Db.GetTransactionStatistics(TransactionType.Acceptance, _context.Basar, parameter.SellerId);
            SettlementStatistics = await _context.Db.GetTransactionStatistics(TransactionType.Settlement, _context.Basar, parameter.SellerId);
            Products = await _context.Db.GetProductsForSeller(_context.Basar, parameter.SellerId).AsNoTracking().ToListAsync();
            Stats = await _context.Db.GetSellerStatisticsAsync(_context.Basar, Seller.Id);

            return Page();
        }
        public VeloPage GetAcceptanceDetailsPage(TransactionStatistic item) => this.GetPage<Acceptances.DetailsModel>(new Acceptances.DetailsParameter { AcceptanceId = item.Transaction.Id });
        public VeloPage GetCreateLabelsPage() => this.GetPage<Labels.CreateAndPrintForSellerModel>(new Labels.CreateAndPrintForSellerParameter { SellerId = Seller.Id });
        public VeloPage GetCreateSettlementPage() => this.GetPage<Settlements.CreateAndPrintModel>(new Settlements.CreateAndPrintParameter { SellerId = Seller.Id });
        public VeloPage GetDocumentPage(ProductsTransaction transaction) => this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = transaction.DocumentId.Value });
        public VeloPage GetEditPage() => this.GetPage<EditModel>(new EditParameter { SellerId = Seller.Id });
        public VeloPage GetStartAcceptancePage() => this.GetPage<Acceptances.StartWithSellerModel>(new Acceptances.StartWithSellerParameter { SellerId = Seller.Id });
    }
}
