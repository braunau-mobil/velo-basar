using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class DetailsParameter
    {
        public int SaleId { get; set; }
        public bool? ShowSuccess { get; set; }
        public bool? OpenDocument { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly ITransactionContext _transactionContext;

        public DetailsModel(ITransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        public ProductsTransaction Sale { get; set; }
        public bool ShowSuccess { get; set; }
        public bool OpenDocument { get; set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            ShowSuccess = parameter.ShowSuccess ?? false;
            OpenDocument = parameter.OpenDocument ?? false;

            Sale = await _transactionContext.GetAsync(parameter.SaleId);
        }
        public VeloPage GetShowFilePage() => this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = Sale.DocumentId.Value });
    }
}
