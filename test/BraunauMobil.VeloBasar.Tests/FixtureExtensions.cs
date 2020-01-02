using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class FixtureExtensions
    {
        public static IEnumerable<Product> CreateManyProducts(this Fixture fixture, int count, Brand brand, ProductType productType, decimal? price = null)
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
            return builder.CreateMany(count);
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
    }
}
