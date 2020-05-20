using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class ModelAssert
    {
        public static void ProductState(StorageState storageState, ValueState valueState, IEnumerable<Product> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            foreach (var product in products)
            {
                ProductState(storageState, valueState, product);
            }
        }
        public static void ProductState(StorageState storageState, ValueState valueState, Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            Assert.Equal(storageState, product.StorageState);
            Assert.Equal(valueState, product.ValueState);
        }
    }
}
