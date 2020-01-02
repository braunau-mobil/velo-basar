using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class FixtureExtensions
    {
        public static IEnumerable<Product> CreateManyProducts(this Fixture fixture, int count, Brand brand, ProductType productType)
        {
            Contract.Requires(fixture != null);
            Contract.Requires(brand != null);
            Contract.Requires(productType != null);

            return fixture.Build<Product>()
                .With(p => p.Brand, brand)
                .With(p => p.BrandId, brand.Id)
                .With(p => p.Type, productType)
                .With(p => p.TypeId, productType.Id)
                .CreateMany(count);
        }
    }
}
