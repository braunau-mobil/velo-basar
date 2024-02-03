using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.CountryExtensionsTests;

public class DefaultOrder
{
    [Fact]
    public void ShouldOrderByIso3166Alpha3Code()
    {
        // Arrange
        IQueryable<CountryEntity> countries = new List<CountryEntity>()
        {
            new () { Iso3166Alpha3Code = "DEU" },
            new () { Iso3166Alpha3Code = "AUT" },
            new () { Iso3166Alpha3Code = "CHE" }
        }.AsQueryable();

        // Act
        CountryEntity[] result = countries.DefaultOrder().ToArray();

        // Assert
        result.Should().BeInAscendingOrder(c => c.Iso3166Alpha3Code);
    }
}
