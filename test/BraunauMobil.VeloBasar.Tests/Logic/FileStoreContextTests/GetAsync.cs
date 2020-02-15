using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.FileStoreContextTests
{
    public class GetAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void Exists()
        {
            await RunOnInitializedDb(async db =>
            {
                var fixture = new Fixture();
                var file = fixture.Build<FileData>().Without(f => f.Id).Create<FileData>();

                db.Files.Add(file);
                await db.SaveChangesAsync();

                var getResult = await FileStoreContext.GetAsync(file.Id);
                Assert.NotNull(getResult);
                Assert.Equal(file.Id, getResult.Id);
                Assert.Equal(file.ContentType, getResult.ContentType);
                Assert.Equal(file.Data, getResult.Data);
            });
        }
        [Fact]
        public async void NotExists()
        {
            await RunOnInitializedDb(async db =>
            {
                var getResult = await FileStoreContext.GetAsync(123);
                Assert.Null(getResult);
            });
        }
    }
}
