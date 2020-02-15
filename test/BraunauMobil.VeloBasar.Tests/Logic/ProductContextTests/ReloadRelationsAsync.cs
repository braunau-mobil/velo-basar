using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductContextTests
{
    public class ReloadRelationsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task CreateProductSerializeToJsonAndAccept()
        {
            var json = "";
            var fixture = new Fixture();

            await RunOnInitializedDb(
                async () =>
                {
                    var basar = await BasarContext.CreateAsync(fixture.Build<Basar>().Without(b => b.Id).Create());
                    var country = await CountryContext.CreateAsync(fixture.Create<Country>());

                    var seller = new Seller
                    {
                        FirstName = "Quark",
                        LastName = "",
                        City = "Deep Space Nine",
                        Street = "",
                        ZIP = "",
                        Country = country
                    };
                    await SellerContext.CreateAsync(seller);

                    var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());

                    var productType = await ProductTypeContext.CreateAsync(fixture.Create<
                        ProductType>());

                    var products = fixture.Build<Product>()
                        .With(p => p.Basar, basar)
                        .With(p => p.BasarId, basar.Id)
                        .With(p => p.Brand, brand)
                        .With(p => p.Type, productType)
                        .With(p => p.BrandId, brand.Id)
                        .With(p => p.TypeId, productType.Id)
                        .CreateMany(2);
                    json = JsonConvert.SerializeObject(products);
                }
            );

            await RunOnInitializedDb(
               async () =>
               {
                   var basar = await BasarContext.GetAsync(1);

                   var products = JsonConvert.DeserializeObject<List<Product>>(json);

                   await ProductContext.ReloadRelationsAsync(products);
                   var acceptance = await TransactionContext.AcceptProductsAsync(basar, 1, products);

                   Assert.NotNull(acceptance);
                   Assert.Equal(1, acceptance.Id);
               }
           );
        }
    }
}
