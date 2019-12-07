using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests.Data.VeloBasarContextTests
{
    [TestClass]
    public class ReloadRelationsAsync : TestWithSqliteDb
    {
        [TestMethod]
        public async Task CreateProductSerializeToJsonAndAccept()
        {
            var json = "";

            await RunOn(
                async ctx =>
                {
                    var basar = new Basar
                    {
                        Date = new DateTime(2063, 4, 5),
                        Name = "Ship Basar",
                        Location = "Depp Space Nine"
                    };
                    ctx.Basar.Add(basar);
                    var country = new Country
                    {
                        Name = "Federation",
                        Iso3166Alpha3Code = "FED"
                    };
                    ctx.Country.Add(country);
                    var seller = new Seller
                    {
                        FirstName = "Quark",
                        LastName = "",
                        City = "Deep Space Nine",
                        Street = "",
                        ZIP = "",
                        Country = country
                    };
                    ctx.Seller.Add(seller);
                    var brand = new Brand
                    {
                        Name = "Brand_1",
                        State = ObjectState.Enabled
                    };
                    ctx.Brand.Add(brand);
                    var productType = new ProductType
                    {
                        Name = "ProductType_1",
                        State = ObjectState.Enabled
                    };
                    ctx.ProductTypes.Add(productType);

                    ctx.SaveChanges();

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
               async ctx =>
               {
                   var basar = await ctx.Basar.GetAsync(1);
                   var seller = await ctx.Seller.GetAsync(1);

                   var products = JsonConvert.DeserializeObject<List<Product>>(json);
                   await ctx.ReloadRelationsAsync(products);

                   var acceptance = await ctx.AcceptProductsAsync(basar, seller, new PrintSettings(), products);
                   Assert.IsNotNull(acceptance);
                   Assert.AreEqual(1, acceptance.Id);
               }
           );
        }
    }
}
