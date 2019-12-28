using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Printing.PdfPrintServiceTests
{
    public sealed class Settlement
    {
        [Fact]
        public static void TwoProdcuts()
        {
            var settlement = new ProductsTransaction()
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
                    BIC = "ABC123456789",
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
            foreach (var productToTransaction in settlement.Products)
            {
                productToTransaction.Transaction = settlement;
            }

            var factory = new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<SharedResource>(factory);

            var creator = new PdfPrintService(localizer);
            var doc = creator.CreateSettlement(settlement, new PrintSettings());
            Assert.NotNull(doc);
        }
    }
}
