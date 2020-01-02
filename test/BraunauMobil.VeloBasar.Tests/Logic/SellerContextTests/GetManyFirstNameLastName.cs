using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SellerContextTests
{
    public class GetManyFirstNameLastName : TestWithServicesAndDb
    {
        [Fact]
        public async Task CaseSensitivness()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                await SetupContext.InitializeDatabaseAsync(new VeloBasar.Models.InitializationConfiguration());

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());

                var seller = await SellerContext.CreateAsync(fixture.Build<Seller>()
                    .With(s => s.FirstName, "Test").Create());

                var sellers = await SellerContext.GetMany("test", null).ToArrayAsync();
                Assert.Single(sellers);
            });
        }
    }
}
