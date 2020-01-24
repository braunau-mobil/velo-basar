using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class ModelAssert
    {
        public static void ProductState(StorageState storageState, ValueState valueState, IEnumerable<Product> products)
        {
            Contract.Requires(products != null);

            foreach (var product in products)
            {
                ProductState(storageState, valueState, product);
            }
        }
        public static void ProductState(StorageState storageState, ValueState valueState, Product product)
        {
            Contract.Requires(product != null);

            Assert.Equal(storageState, product.StorageState);
            Assert.Equal(valueState, product.ValueState);
        }
    }
}
