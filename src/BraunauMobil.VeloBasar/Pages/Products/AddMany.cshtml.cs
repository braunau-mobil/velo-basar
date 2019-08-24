using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class AddManyModel : BasarPageModel
    {
        public AddManyModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public int? AcceptanceId { get; set; }

        public Seller Seller { get; set; }

        [BindProperty]
        public PartialValidatedList<Product> Products { get; set; }
        
        public async Task OnGetAsync(int basarId, int sellerId, int? acceptanceId)
        {
            await LoadBasarAsync(basarId);
            await LoadSellerAsync(sellerId);

            Products = new PartialValidatedList<Product>();
            var productCount = 10;
            for (var count = 0; count < productCount; count++)
            {
                Products.Add(new Product() { Price = 1 });
            }

            AcceptanceId = acceptanceId;
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int sellerId, bool? addAdditional, int? acceptanceId)
        {
            await LoadBasarAsync(basarId);
            await LoadSellerAsync(sellerId);

            if (!ModelState.IsValid)
            {
                return Page();
            }
             
            var acceptance = await Context.AcceptProductsAsync(Basar, sellerId, acceptanceId, Products.Where(p => !p.IsEmtpy()).ToArray());

            if (addAdditional == true)
            {
                return RedirectToPage("/Products/AddMany", new { basarId = Basar.Id, sellerId, acceptanceId = acceptance.Id });
            }

            return RedirectToPage("/Acceptances/Details", new { basarId = Basar.Id, acceptanceId = acceptance.Id });
        }

        private async Task LoadSellerAsync(int sellerId)
        {
            Seller = await Context.Seller.FirstOrDefaultAsync(s => s.Id == sellerId);
        }
    }
}
