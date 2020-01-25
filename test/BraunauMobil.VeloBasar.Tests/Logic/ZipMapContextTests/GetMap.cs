using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ZipMapContextTests
{
    public class GetMap : TestWithServicesAndDb
    {
        [Fact]
        public async Task Test()
        {
            await RunOnInitializedDb(async db =>
            {
                await SetupContext.InitializeDatabaseAsync(new InitializationConfiguration());
                var fixture = new Fixture();
                var autZips = fixture.Build<ZipMap>()
                    .With(zm => zm.CountryId, 1)
                    .CreateMany(4);
                db.ZipMap.AddRange(autZips);
                var gerZips = fixture.Build<ZipMap>()
                    .With(zm => zm.CountryId, 2)
                    .CreateMany(10);
                db.ZipMap.AddRange(gerZips);
                await db.SaveChangesAsync();

                var map = ZipMapContext.GetMap();
                foreach (var exptectedMap in autZips)
                {
                    Assert.True(map[1].ContainsKey(exptectedMap.Zip));
                    Assert.Equal(exptectedMap.City, map[1][exptectedMap.Zip]);
                }
                foreach (var exptectedMap in gerZips)
                {
                    Assert.True(map[2].ContainsKey(exptectedMap.Zip));
                    Assert.Equal(exptectedMap.City, map[2][exptectedMap.Zip]);
                }
            });
        }
    }
}
