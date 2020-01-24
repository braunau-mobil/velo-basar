using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SellerContextTests
{
    public class SetValueStateAsync : TestWithServicesAndDb
    {
        [Theory]
        [InlineData(ValueState.Settled, ValueState.Settled)]
        [InlineData(ValueState.NotSettled, ValueState.NotSettled)]
        public async Task Test(ValueState setTo, ValueState expectedState)
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                await SetupContext.InitializeDatabaseAsync(new VeloBasar.Models.InitializationConfiguration());

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());

                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                await SellerContext.SetValueStateAsync(seller.Id, setTo);
                Assert.Equal(expectedState, seller.ValueState);
            });
        }
    }
}
