using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.BrandContext
{
    public class GetSelectList : TestWithServicesAndDb
    {
        [Fact]
        public async void CheckOrder()
        {
            await RunOnInitializedDb(async () =>
            {
                await BrandContext.CreateAsync(new Brand { Name = "X" });
                await BrandContext.CreateAsync(new Brand { Name = "A" });
                await BrandContext.CreateAsync(new Brand { Name = "M" });

                var items = BrandContext.GetSelectList().Cast<SelectListItem>().ToArray();
                Assert.Equal("A", items[0].Text);
                Assert.Equal("M", items[1].Text);
                Assert.Equal("X", items[2].Text);
            });
        }

    }
}
