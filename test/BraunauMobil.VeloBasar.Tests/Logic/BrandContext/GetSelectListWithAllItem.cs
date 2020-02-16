using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.BrandContext
{
    public class GetSelectListWithAllItem : TestWithServicesAndDb
    {
        [Fact]
        public async void Test()
        {
            await RunOnInitializedDb(async () =>
            {
                await BrandContext.CreateAsync(new Brand { Name = "X" });
                await BrandContext.CreateAsync(new Brand { Name = "A" });
                await BrandContext.CreateAsync(new Brand { Name = "M" });

                var items = BrandContext.GetSelectListWithAllItem().Cast<SelectListItem>().ToArray();
                Assert.Equal("Alle", items[0].Text);
                Assert.Equal("A", items[1].Text);
                Assert.Equal("M", items[2].Text);
                Assert.Equal("X", items[3].Text);
            });
        }
    }
}
