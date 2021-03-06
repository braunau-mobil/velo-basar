﻿using AutoFixture;
using AutoFixture.Dsl;
using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class FixtureExtensions
    {
        public static IPostprocessComposer<Product> BuildProduct(this Fixture fixture, decimal price)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            return fixture.Build<Product>()
                .With(p => p.Price, price)
                .Without(p => p.Id);
        }
        public static IPostprocessComposer<Product> BuildProduct(this Fixture fixture, Brand brand, ProductType productType, decimal? price = null)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (brand == null) throw new ArgumentNullException(nameof(brand));
            if (productType == null) throw new ArgumentNullException(nameof(productType));

            var builder = fixture.Build<Product>()
                .Without(p => p.Id)
                .With(p => p.Brand, brand)
                .With(p => p.BrandId, brand.Id)
                .With(p => p.Type, productType)
                .With(p => p.TypeId, productType.Id);
            if (price != null)
            {
                builder = builder.With(p => p.Price, price.Value);
            }
            return builder;
        }

        public static ProductsTransaction CreateAcceptance(this Fixture fixture, IEnumerable<Product> products)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var tx = fixture.Build<ProductsTransaction>()
                .With(t => t.Products, products.Select(p => new ProductToTransaction() { Product = p, ProductId = p.Id }).ToArray())
                .With(t => t.Type, TransactionType.Acceptance)
                .Create();
            UpdateProductRelations(tx);
            return tx;
        }
        public static Seller CreateSeller(this Fixture fixture, Country country)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (country == null) throw new ArgumentNullException(nameof(country));

            return fixture.Build<Seller>()
                .With(s => s.Country, country)
                .With(s => s.CountryId, country.Id)
                .Create();
        }

        private static void UpdateProductRelations(ProductsTransaction tx)
        {
            foreach (var product in tx.Products)
            {
                product.Transaction = tx;
                product.TransactionId = tx.Id;
            }
        }
    }
}
