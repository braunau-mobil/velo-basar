using AutoFixture;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class GetProductsForSeller : TestWithServicesAndDb
    {
        public GetProductsForSeller()
        {
            AddLocalization();
            AddLogic();
        }

        [Fact]
        public async Task MultipleAcceptances()
        {
            await RunWithServiesAndDb(async serviceProvider =>
            {
                var setupContext = serviceProvider.GetRequiredService<ISetupContext>();
                await setupContext.InitializeDatabaseAsync(new InitializationConfiguration());
                
                var basarContext = serviceProvider.GetRequiredService<IBasarContext>();
                var basar = await basarContext.CreateAsync(new Basar());

                var fixture = new Fixture();
                var sellerContext = serviceProvider.GetRequiredService<ISellerContext>();
                var seller = await sellerContext.CreateAsync(fixture.Create<Seller>());

                var transactionContext = serviceProvider.GetRequiredService<ITransactionContext>();
                var firstAcceptance = transactionContext.AcceptProductsAsync(basar, seller.Id, fixture.CreateMany<Product>(1).ToList());
                var secondAceptance = transactionContext.AcceptProductsAsync(basar, seller.Id, fixture.CreateMany<Product>(2).ToList());

                var productContext = serviceProvider.GetRequiredService<IProductContext>();
                var products = await productContext.GetProductsForSeller(basar, seller.Id).ToArrayAsync();
                Assert.Equal(3, products.Length);
            });
        }
    }
}
