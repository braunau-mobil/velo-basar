using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.FileStoreContextTests
{
    public class CreateProductLabelAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void Test()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();
                var basar = await BasarContext.CreateAsync(new Basar());

                var product = fixture.Build<Product>().Without(p => p.LabelId).Create();
                var fileId = await FileStoreContext.CreateProductLabelAsync(product, new PrintSettings());

                Assert.NotEqual(0, fileId);

                var file = await FileStoreContext.GetAsync(fileId);
                Assert.NotNull(file);

                PdfAssert.PageCount(1, file.Data);
            });
        }
    }
}
