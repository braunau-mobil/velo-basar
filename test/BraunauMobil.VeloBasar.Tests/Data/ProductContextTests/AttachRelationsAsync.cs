using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductContextTests
{
    [TestClass]
    public class AttachRelationsAsync : TestWithSqliteDb
    {
        [TestMethod]
        public async Task CreateProductSerializeToJsonAndAccept()
        {
            var json = "";

            await RunOn(
                async db =>
                {
                    var basar = new Basar
                    {
                        Date = new DateTime(2063, 4, 5),
                        Name = "Ship Basar",
                        Location = "Depp Space Nine"
                    };
                    db.Basars.Add(basar);
                    var country = new Country
                    {
                        Name = "Federation",
                        Iso3166Alpha3Code = "FED"
                    };
                    db.Countries.Add(country);
                    var seller = new Seller
                    {
                        FirstName = "Quark",
                        LastName = "",
                        City = "Deep Space Nine",
                        Street = "",
                        ZIP = "",
                        Country = country
                    };
                    db.Sellers.Add(seller);
                    var brand = new Brand
                    {
                        Name = "Brand_1",
                        State = ObjectState.Enabled
                    };
                    db.Brands.Add(brand);
                    var productType = new ProductType
                    {
                        Name = "ProductType_1",
                        State = ObjectState.Enabled
                    };
                    db.ProductTypes.Add(productType);

                    await db.SaveChangesAsync();

                    var product = new Product
                    {
                        Brand = brand,
                        Color = "Red",
                        Description = "Bla Bla Bla",
                        FrameNumber = "1234",
                        Price = 666.66m,
                        TireSize = "TireSzie_1",
                        Type = productType
                    };

                    var products = new List<Product>
                    {
                        product
                    };
                    json = JsonConvert.SerializeObject(products);
                }
            );

            await RunOn(
               async db =>
               {
                   var fileStoreContext = new FileStoreContext(db);
                   var productContext = new ProductContext(db);
                   var settingsContext = new SettingsContext(db, fileStoreContext);
                   await settingsContext.UpdateAsync(new VeloSettings());
                   await settingsContext.UpdateAsync(new PrintSettings());
                   var sellerContext = new SellerContext(db);
                   var transactionContext = new TransactionContext(db, this, new PdfPrintService(TestUtils.CreateLocalizer()), productContext, settingsContext, fileStoreContext, sellerContext); 
                   var basarContext = new BasarContext(db, TestUtils.CreateLocalizer(), this, settingsContext);

                   var basar = await basarContext.GetAsync(1);

                   var products = JsonConvert.DeserializeObject<List<Product>>(json);

                   productContext.AttachRelations(products);
                   var acceptance = await transactionContext.AcceptProductsAsync(basar, 1, products);

                   Assert.IsNotNull(acceptance);
                   Assert.AreEqual(1, acceptance.Id);
               }
           );
        }
    }
}
