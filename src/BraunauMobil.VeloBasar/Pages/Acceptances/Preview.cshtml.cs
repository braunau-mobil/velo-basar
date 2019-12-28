using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class PreviewModel : PageModel
    {
        private readonly ISettingsContext _settingsContext;
        private readonly IPrintService _printService;

        public PreviewModel(ISettingsContext settingsContext, IPrintService printService)
        {
            _settingsContext = settingsContext;
            _printService = printService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _settingsContext.GetPrintSettingsAsync();
            var bytes = _printService.CreateAcceptance(SampleAcceptance(), settings);
            return File(bytes, "application/pdf");
        }

        private ProductsTransaction SampleAcceptance()
        {
            return new ProductsTransaction()
            {
                Basar = new Basar
                {
                    Date = new DateTime(2063, 04, 05),
                    Location = "Hopfenhause",
                    Name = "Testbasar",
                    ProductCommission = 0.1m
                },
                Notes = "Bla Bla Bla Anmerkung...",
                Number = 55,
                Products = new List<ProductToTransaction>
                {
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Brand = new Brand
                            {
                                Name = "KTM"
                            },
                            Color = "Rot mit grünen Streifen",
                            Description = "Ganz tolles Rad, leider Fehlt der Sattel",
                            FrameNumber = "123498zdsfvh48",
                            Price = 12.45m,
                            TireSize = "Sehgr groß",
                            Type = new ProductType
                            {
                                Name = "City-Bike"
                            }
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Brand = new Brand
                            {
                                Name = "KTM"
                            },
                            Color = "Rot mit grünen Streifen",
                            Description = "Ganz tolles Rad, leider Fehlt der Sattel",
                            FrameNumber = "123498zdsfvh48",
                            Price = 12.45m,
                            TireSize = "Sehgr groß0",
                            Type = new ProductType
                            {
                                Name = "City-Bike"
                            }
                        }
                    }
                },
                Seller = new Seller
                {
                    BankAccountHolder = "Bilbo Beutlin",
                    BIC = "ABC123456789",
                    City = "Hopfenhause",
                    Country = new Country
                    {
                        Name = "Österreich",
                        Iso3166Alpha3Code = "AUT"
                    },
                    FirstName = "Bilbo",
                    IBAN = "AT00123412341234",
                    LastName = "Beutlin",
                    Street = "Biergasse 12",
                    Token = "GHTGF4",
                    ZIP = "1234"
                }
            };
        }
    }
}