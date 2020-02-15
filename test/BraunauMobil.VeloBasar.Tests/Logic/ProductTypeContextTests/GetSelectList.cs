using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductTypeContextTests
{
    public class GetSelectList : TestWithServicesAndDb
    {
        [Fact]
        public async void CheckOrder()
        {
            await RunOnInitializedDb(async () =>
            {
                await ProductTypeContext.CreateAsync(new ProductType { Name = "X" });
                await ProductTypeContext.CreateAsync(new ProductType { Name = "A" });
                await ProductTypeContext.CreateAsync(new ProductType { Name = "M" });

                var items = ProductTypeContext.GetSelectList().Cast<SelectListItem>().ToArray();
                Assert.Equal("A", items[0].Text);
                Assert.Equal("M", items[1].Text);
                Assert.Equal("X", items[2].Text);
            });
        }

    }
}
