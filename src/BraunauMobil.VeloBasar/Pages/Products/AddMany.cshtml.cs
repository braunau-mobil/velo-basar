using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class AddManyModel : BasarPageModel
    {
        public AddManyModel(VeloBasarContext context) : base(context)
        {
        }

        public Seller Seller { get; set; }

        [BindProperty]
        public PartialValidatedList<Product> Products { get; set; }

        public async Task OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);
            await LoadSellerAsync(sellerId);

            Products = new PartialValidatedList<Product>();
            var productCount = 10;
            for (var count = 0; count < productCount; count++)
            {
                Products.Add(new Product() { Price = 1 });
            }
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);
            await LoadSellerAsync(sellerId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var acceptance = new Acceptance
            {
                Basar = Basar,
                Seller = Seller,
                TimeStamp = DateTime.Now,
                Products = new List<ProductAcceptance>(),
            };

            foreach (var product in Products.Where(p => !p.IsEmtpy()))
            {
                await Context.Product.AddAsync(product);
                await Context.SaveChangesAsync();

                var productAcceptance = new ProductAcceptance
                {
                    Acceptance = acceptance,
                    Product = product
                };
                acceptance.Products.Add(productAcceptance);
            }

            acceptance.Number = Context.NextNumber(basarId, TransactionType.Acceptance);
            await Context.Acceptance.AddAsync(acceptance);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Sellers/Details", new { basarId = Basar.Id, sellerId = Seller.Id });
        }

        private async Task LoadSellerAsync(int sellerId)
        {
            Seller = await Context.Seller.FirstOrDefaultAsync(s => s.Id == sellerId);
        }
    }
}
