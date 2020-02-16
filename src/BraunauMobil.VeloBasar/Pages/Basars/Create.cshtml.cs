using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class CreateModel : PageModel
    {
        private readonly IBasarContext _basarContext;

        public CreateModel(IBasarContext basarContext)
        {
            _basarContext = basarContext;
            BasarToCreate = new Basar
            {
                Date = DateTime.Now
            };
        }

        [BindProperty]
        public Basar BasarToCreate { get; set; }
        [Display(Name = "Artikel Provision in Prozent")]
        [Range(0, 100, ErrorMessage = "Bitte einen Wert zwischen 0 und 100 % eingeben")]
        [BindProperty]
        public int ProductCommissionPercentage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            BasarToCreate.ProductCommission = ProductCommissionPercentage / 100.0m;
            await _basarContext.CreateAsync(BasarToCreate);
            return this.RedirectToPage<ListModel>();
        }
    }
}