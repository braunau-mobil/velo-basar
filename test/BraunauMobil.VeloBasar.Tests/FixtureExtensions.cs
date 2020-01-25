using AutoFixture;
using AutoFixture.Dsl;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class FixtureExtensions
    {
        public static IPostprocessComposer<Product> BuildProduct(this Fixture fixture, decimal price)
        {
            Contract.Requires(fixture != null);

            return fixture.Build<Product>()
                .With(p => p.Price, price)
                .Without(p => p.Id);
        }
        public static IPostprocessComposer<Product> BuildProduct(this Fixture fixture, Brand brand, ProductType productType, decimal? price = null)
        {
            Contract.Requires(fixture != null);
            Contract.Requires(brand != null);
            Contract.Requires(productType != null);

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
            Contract.Requires(fixture != null);

            var tx = fixture.Build<ProductsTransaction>()
                .With(t => t.Products, products.Select(p => new ProductToTransaction() { Product = p, ProductId = p.Id }).ToArray())
                .With(t => t.Type, TransactionType.Acceptance)
                .Create();
            UpdateProductRelations(tx);
            return tx;
        }
        public static Seller CreateSeller(this Fixture fixture, Country country)
        {
            Contract.Requires(fixture != null);
            Contract.Requires(country != null);

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
