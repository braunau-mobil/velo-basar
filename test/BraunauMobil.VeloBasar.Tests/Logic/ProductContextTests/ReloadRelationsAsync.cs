using AutoFixture;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductContextTests
{
    public class ReloadRelationsAsync : TestWithServicesAndDb
    {
        public ReloadRelationsAsync()
        {
            AddLocalization();
            AddLogic();
        }

        [Fact]
        public async Task CreateProductSerializeToJsonAndAccept()
        {
            var json = "";
            var fixture = new Fixture();

            await RunWithServiesAndDb(
                async serviceProvider =>
                {
                    var setupContext = serviceProvider.GetRequiredService<ISetupContext>();
                    await setupContext.InitializeDatabaseAsync(new InitializationConfiguration());

                    var basarContext = serviceProvider.GetRequiredService<IBasarContext>();
                    var basar = await basarContext.CreateAsync(fixture.Build<Basar>().Without(b => b.Id).Create());

                    var countryContext = serviceProvider.GetRequiredService<ICountryContext>();
                    var country = await countryContext.CreateAsync(fixture.Create<Country>());

                    var sellerContext = serviceProvider.GetRequiredService<ISellerContext>();
                    var seller = new Seller
                    {
                        FirstName = "Quark",
                        LastName = "",
                        City = "Deep Space Nine",
                        Street = "",
                        ZIP = "",
                        Country = country
                    };
                    await sellerContext.CreateAsync(seller);

                    var brandContext = serviceProvider.GetRequiredService<IBrandContext>();
                    var brand = await brandContext.CreateAsync(fixture.Create<Brand>());

                    var productTypeContext = serviceProvider.GetRequiredService<IProductTypeContext>();
                    var productType = await productTypeContext.CreateAsync(fixture.Create<
                        ProductType>());

                    var products = fixture.Build<Product>()
                        .With(p => p.Brand, brand)
                        .With(p => p.Type, productType)
                        .With(p => p.BrandId, brand.Id)
                        .With(p => p.TypeId, productType.Id)
                        .CreateMany(2);
                    json = JsonConvert.SerializeObject(products);
                }
            );

            await RunWithServiesAndDb(
               async serviceProvider =>
               {
                   var transactionContext = serviceProvider.GetRequiredService<ITransactionContext>();
                   var basarContext = serviceProvider.GetRequiredService<IBasarContext>();
                   var productContext = serviceProvider.GetRequiredService<IProductContext>();
                   
                   var basar = await basarContext.GetAsync(1);

                   var products = JsonConvert.DeserializeObject<List<Product>>(json);

                   await productContext.ReloadRelationsAsync(products);
                   var acceptance = await transactionContext.AcceptProductsAsync(basar, 1, products);

                   Assert.NotNull(acceptance);
                   Assert.Equal(1, acceptance.Id);
               }
           );
        }
    }
}
