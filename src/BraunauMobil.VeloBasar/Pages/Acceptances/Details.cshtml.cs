using System.Threading.Tasks;
using BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
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
        private readonly VeloBasarContext _context;

        public DetailsModel(VeloBasarContext context)
        {
            _context = context;
        }

        public ProductsTransaction Acceptance { get; set; }
        public DetailsParameter Parameter { get; private set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            Parameter = parameter;
            Acceptance = await _context.Transactions.GetAsync(parameter.AcceptanceId);
        }
        public VeloPage GetSellerDetailsPage() => this.GetPage<Sellers.DetailsModel>(new Sellers.DetailsParameter { SellerId = Acceptance.SellerId.Value });
    }
}
