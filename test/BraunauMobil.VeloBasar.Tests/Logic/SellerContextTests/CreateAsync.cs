using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SellerContextTests
{
    public class CreateAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void Test()
        {
            await RunOnInitializedDb(async () => 
            {
                var fixture = new Fixture();

                var country = await CountryContext.CreateAsync(fixture.Create<Country>());

                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));

                Assert.NotNull(seller);
                Assert.NotEqual(0, seller.Id);
                Assert.NotNull(seller.Token);
            });
        }
    }
}
