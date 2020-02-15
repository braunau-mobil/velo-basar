using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.FileStoreContextTests
{
    public class DeleteAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void Test()
        {
            await RunOnInitializedDb(async db =>
            {
                var fixture = new Fixture();
                var file = fixture.Create<FileData>();

                db.Files.Add(file);
                await db.SaveChangesAsync();

                await FileStoreContext.DeleteAsync(file.Id);

                Assert.False(await FileStoreContext.ExistsAsync(file.Id));
            });
        }
    }
}
