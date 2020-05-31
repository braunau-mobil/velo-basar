﻿using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductsTransactionTests
{
    public class GetPayoutCommissionTotal
    {
        [Fact]
        public void Test()
        {
            var tx = new ProductsTransaction
            {
                Basar = new Basar
                {
                    ProductCommission = 0.1m
                },
                Products = new List<ProductToTransaction>
                {
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 123.12m,
                            StorageState = StorageState.Sold
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 99.99m,
                            StorageState = StorageState.Sold
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 100.0m,
                            StorageState = StorageState.Available
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 100.0m,
                            StorageState = StorageState.Gone
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 100.0m,
                            StorageState = StorageState.Locked
                        }
                    }
                }
            };
            Assert.Equal(32.311m, tx.GetPayoutCommissionTotal());
        }
    }
}
