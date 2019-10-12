using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CreateOrEditModel : BasarPageModel
    {
        public CreateOrEditModel(VeloBasarContext context) : base(context)
        {
        }

        public string ErrorText { get; set; }

        public ProductsTransaction Sale { get; set; }

        [BindProperty]
        public IList<Product> Products { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Artikel Id")]
        public int ProductId { get; set; }

        public async Task OnGetAsync(int basarId, int? saleId, string errorText)
        {
            await LoadBasarAsync(basarId);

            if (saleId != null)
            {
                Sale = await Context.GetSaleAsync(saleId.Value);
                Products = Sale.Products.Select(ps => ps.Product).ToList();
            }
            else
            {
                Products = new PaginatedList<Product>();
            }

            ErrorText = errorText;
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int? saleId)
        {
            TryValidateModel(ProductId);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await LoadBasarAsync(basarId);

            var product = await Context.GetProductAsync(ProductId);
            if (product == null)
            {
                return RedirectToPage("/Sales/CreateOrEdit", new { basarId, saleId, errorText = $"Es wurde kein Produkt mit der id {ProductId} gefunden" });
            }
            if (!product.CanSell())
            {
                return RedirectToPage("/Sales/CreateOrEdit", new { basarId, saleId, errorText = $"Das Produkt {product.Brand} {product.Type} mit der id {ProductId} ist nicht mehr verfügbar." });
            }

            var sale = await Context.AddProductToSaleAsync(Basar, saleId, product);

            return RedirectToPage("/Sales/CreateOrEdit", new { basarId, saleId = sale.Id });
        }
    }
}