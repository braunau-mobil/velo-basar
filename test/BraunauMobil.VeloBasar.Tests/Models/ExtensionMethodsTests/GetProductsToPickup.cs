using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests
{
    public class GetProductsToPickup
    {
        [Fact]
        public void Test()
        {
            var fixture = new Fixture();

            var products = fixture.BuildProduct(1m).CreateMany(4).ToArray();
            products[0].StorageState = StorageState.Available;
            products[1].StorageState = StorageState.Gone;
            products[2].StorageState = StorageState.Locked;
            products[3].StorageState = StorageState.Sold;

            var result = products.GetProductsToPickup().ToArray();
            ModelAssert.ProductState(StorageState.Available, ValueState.NotSettled, result[0]);
            ModelAssert.ProductState(StorageState.Locked, ValueState.NotSettled, result[1]);
        }
    }
}
