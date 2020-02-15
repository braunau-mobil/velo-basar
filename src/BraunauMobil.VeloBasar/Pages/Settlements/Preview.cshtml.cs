using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Settlements
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
            var bytes = _printService.CreateTransaction(SampleSettlement(), settings);
            return File(bytes, "application/pdf");
        }

        private static ProductsTransaction SampleSettlement()
        {
            var tx = new ProductsTransaction()
            {
                Type = TransactionType.Settlement,
                Basar = new Basar
                {
                    Date = new DateTime(2063, 04, 05),
                    Location = "Hopfenhausen",
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
                            Id = 12,
                            Brand = new Brand
                            {
                                Name = "KTM"
                            },
                            Color = "Rot mit grünen Streifen",
                            Description = "Ganz tolles Rad, leider Fehlt der Sattel",
                            FrameNumber = "123498zdsfvh48",
                            Price = 12.45m,
                            TireSize = "55\"",
                            Type = new ProductType
                            {
                                Name = "City-Bike"
                            },
                            StorageState = StorageState.Sold
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Id = 22,
                            Brand = new Brand
                            {
                                Name = "KTM"
                            },
                            Color = "Rot mit grünen Streifen",
                            Description = "Ganz tolles Rad, leider Fehlt der Sattel",
                            FrameNumber = "123498zdsfvh48",
                            Price = 99.21m,
                            TireSize = "12\"",
                            Type = new ProductType
                            {
                                Name = "City-Bike"
                            },
                            StorageState = StorageState.Available
                        }                        
                    }
                },
                Seller = new Seller
                {
                    BankAccountHolder = "Bilbo Beutlin",
                    City = "Hopfenhausen",
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
                },
                TimeStamp = new DateTime(2063, 04, 05, 15, 22, 11)
            };
            foreach (var productToTransaction in tx.Products)
            {
                productToTransaction.Transaction = tx;
            }
            return tx;
        }
    }
}