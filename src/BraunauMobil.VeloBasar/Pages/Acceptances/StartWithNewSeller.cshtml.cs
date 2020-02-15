using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

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
        private readonly ISellerContext _sellerContext;
        private readonly ICountryContext _countryContext;
        private readonly IZipMapContext _zipMapContext;
        private bool _isValidationEnabled;

        public StartWithNewSellerModel(IVeloContext context, ISellerContext sellerContext, ICountryContext countryContext, IZipMapContext zipMapContext)
        {
            _context = context;
            _sellerContext = sellerContext;
            _countryContext = countryContext;
            _zipMapContext = zipMapContext;
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
            Contract.Requires(parameter != null);

            ViewData["Countries"] = _countryContext.GetSelectList();

            if (parameter.SellerId != null)
            {
                Seller = await _sellerContext.GetAsync(parameter.SellerId.Value);
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
            Contract.Requires(parameter != null);

            ViewData["Countries"] = _countryContext.GetSelectList();

            if (parameter.Search == true)
            {
                if (string.IsNullOrEmpty(Seller.FirstName) && string.IsNullOrEmpty(Seller.LastName))
                {
                    ErrorText = _context.Localizer["Bitte Vor und/oder Nachnamen eingeben für die Suche"];
                    return Page();
                }

                var sellers = await _sellerContext.GetMany(Seller.FirstName, Seller.LastName).ToListAsync();
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

                return Page();
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
                await _sellerContext.UpdateAsync(Seller);
            }
            else
            {
                await _sellerContext.CreateAsync(Seller);
            }

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
        public bool IsValidationEnabled()
        {
            return _isValidationEnabled;
        }
        public IDictionary<int, IDictionary<string, string>> GetZipMap() => _zipMapContext.GetMap();
    }
}