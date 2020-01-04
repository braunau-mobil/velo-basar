using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class DetailsParameter
    {
        public int AcceptanceId { get; set; }
        public bool? ShowSuccess { get; set; }
        public bool? OpenDocument { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly ITransactionContext _context;

        public DetailsModel(ITransactionContext context)
        {
            _context = context;
        }

        public ProductsTransaction Acceptance { get; set; }
        public DetailsParameter Parameter { get; private set; }
        public ProductsViewModel Products { get; private set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            Parameter = parameter;
            Acceptance = await _context.GetAsync(parameter.AcceptanceId);
            Products = new ProductsViewModel(Acceptance.Products)
            {
                ShowFooter = true,
                FooterValue = Acceptance.GetProductsSum(),
                ShowSeller = false
            };
        }
        public VeloPage GetSellerDetailsPage() => this.GetPage<Sellers.DetailsModel>(new Sellers.DetailsParameter { SellerId = Acceptance.SellerId.Value });
    }
}
