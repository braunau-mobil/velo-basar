using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductTypeContextTests
{
    public class GetSelectListWithAllItem : TestWithServicesAndDb
    {
        [Fact]
        public async void Test()
        {
            await RunOnInitializedDb(async () =>
            {
                await ProductTypeContext.CreateAsync(new ProductType { Name = "X" });
                await ProductTypeContext.CreateAsync(new ProductType { Name = "A" });
                await ProductTypeContext.CreateAsync(new ProductType { Name = "M" });

                var items = ProductTypeContext.GetSelectListWithAllItem().Cast<SelectListItem>().ToArray();
                Assert.Equal("Alle", items[0].Text);
                Assert.Equal("A", items[1].Text);
                Assert.Equal("M", items[2].Text);
                Assert.Equal("X", items[3].Text);
            });
        }

    }
}
