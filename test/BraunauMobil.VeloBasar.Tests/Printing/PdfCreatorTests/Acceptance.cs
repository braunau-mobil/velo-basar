using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Printing.PdfCreatorTests
{
    [TestClass]
    public class Acceptance
    {
        [TestMethod]
        public void OneProduct()
        {
            var acceptance = new ProductsTransaction()
            {
                Basar = new Basar
                {
                    Date = new DateTime(2063, 04, 05),
                    Location = "Hopfenhause",
                    Name = "Testbasar",
                    ProductCommission = 0.1m,
                    ProductDiscount = 0.0m,
                    SellerDiscount = 0.0m
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
            
            var factory = new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<SharedResource>(factory);

            var creator = new PdfCreator(localizer);
            var doc = creator.CreateAcceptance(acceptance, new PrintSettings());
            Assert.IsNotNull(doc);
        }
    }
}
