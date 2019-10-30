using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class StartWithNewSellerParameter
    {
        public int? SellerId { get; set; }
        public bool? Search { get; set; } 
    }
    public class StartWithNewSellerModel : PageModel
    {
        private readonly IVeloContext _context;
        private bool _isValidationEnabled;

        public StartWithNewSellerModel(IVeloContext context)
        {
            _context = context;
        }

        public string ErrorText { get; set; }
        [BindProperty]
        public Seller Seller { get; set; }
        public ListViewModel<Seller> Sellers { get; set; }
        [BindProperty]
        public bool AreWeInEditMode { get; set; }
        public bool HasSellers { get => Sellers != null; }

        public async Task<IActionResult> OnGetAsync(StartWithNewSellerParameter parameter)
        {
            ViewData["Countries"] = new SelectList(_context.Db.Country, "Id", "Name");

            if (parameter.SellerId != null)
            {
                Seller = await _context.Db.Seller.GetAsync(parameter.SellerId.Value);
                if (Seller == null)
                {
                    return NotFound();
                }
                _isValidationEnabled = true;
                AreWeInEditMode = true;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(StartWithNewSellerParameter parameter)
        {
            ViewData["Countries"] = new SelectList(_context.Db.Country, "Id", "Name");

            if (parameter.Search == true)
            {
                if (string.IsNullOrEmpty(Seller.FirstName) && string.IsNullOrEmpty(Seller.LastName))
                {
                    ErrorText = _context.Localizer["Bitte Vor und/oder Nachnamen eingeben für die Suche"];
                    return Page();
                }

                var sellers = await _context.Db.Seller.GetMany(Seller.FirstName, Seller.LastName).ToListAsync();
                if (sellers.Count > 0)
                {
                    Sellers = new ListViewModel<Seller>(_context.Basar, sellers, new[]{
                        new ListCommand<Seller>(seller => this.GetPage<StartWithNewSellerModel>(new StartWithNewSellerParameter { SellerId = seller.Id }))
                        {
                            Text = _context.Localizer["Übernehmen"]
                        }
                    });
                }
                else
                {
                    ErrorText = _context.Localizer["Es konnte kein Verkäufer gefunden werden."];
                }
            }

            if (string.IsNullOrEmpty(Seller.FirstName) || string.IsNullOrEmpty(Seller.LastName))
            {
                _isValidationEnabled = true;
                return Page();
            }

            if (!ModelState.IsValid)
            {
                _isValidationEnabled = true;
                return Page();
            }

            if (parameter.SellerId != null)
            {
                _context.Db.Attach(Seller).State = EntityState.Modified;
            }
            else
            {
                _context.Db.Seller.Add(Seller);
            }

            await _context.Db.SaveChangesAsync();
            return this.RedirectToPage<EnterProductsModel>(new EnterProductsParameter { SellerId = Seller.Id });
        }
        public object GetNextParameter()
        {
            if (AreWeInEditMode)
            {
                return new StartWithNewSellerParameter { SellerId = Seller.Id };
            }
            return new object();
        }
        public object GetSearchParameter() => new StartWithNewSellerParameter { Search = true };
        public bool IsValidationEnabled()
        {
            return _isValidationEnabled;
        }
    }
}