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
    public class Sale
    {
        [Fact]
        public void TwoProdcuts()
        {
            var seller = new Seller
            {
                BankAccountHolder = "Bilbo Beutlin",
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
            };

            var sale = new ProductsTransaction()
            {
                Type = TransactionType.Sale,
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
                            Seller = seller,
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
                            Seller = seller,
                            TireSize = "Sehgr groß0",
                            Type = new ProductType
                            {
                                Name = "City-Bike"
                            }
                        }
                    }
                }
            };

            var factory = new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<SharedResource>(factory);

            var creator = new PdfPrintService(localizer);
            var doc = creator.CreateTransaction(sale, new PrintSettings());
            Assert.NotNull(doc);
        }
    }
}
