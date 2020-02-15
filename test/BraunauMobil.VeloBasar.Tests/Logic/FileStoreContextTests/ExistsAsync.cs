using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.FileStoreContextTests
{
    public class ExistsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void Exists()
        {
            await RunOnInitializedDb(async db =>
            {
                var fixture = new Fixture();
                var file = fixture.Create<FileData>();

                db.Files.Add(file);
                await db.SaveChangesAsync();

                Assert.True(await FileStoreContext.ExistsAsync(file.Id));
            });
        }
        [Fact]
        public async void NotExists()
        {
            await RunOnInitializedDb(async db =>
            {
                var fixture = new Fixture();
                var file = fixture.Create<FileData>();

                Assert.False(await FileStoreContext.ExistsAsync(file.Id));
                
                db.Files.Add(file);
                await db.SaveChangesAsync();

                Assert.False(await FileStoreContext.ExistsAsync(file.Id + 1));
            });
        }
    }
}
