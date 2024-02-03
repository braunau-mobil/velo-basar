using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.ZipCollectionTests;

public class Ctor
{
    [Fact]
    public void ShouldLoadZipCodesFromApplicationResoures()
    {
        // Arrange
        IReadOnlyList<CountryEntity> countries = new List<CountryEntity>
        {
            new() { Iso3166Alpha3Code ="AUT", Id = 1 },
            new() { Iso3166Alpha3Code = "GER", Id = 2 }
        };

        // Act
        ZipCollection sut = new (countries);

        // Assert
        using (new AssertionScope())
        {
            sut.Should().ContainEquivalentOf(new ZipCodeEntity { Zip = "3100", City = "St. Pölten", CountryId = 1 });
            sut.Should().ContainEquivalentOf(new ZipCodeEntity { Zip = "4020", City = "Linz", CountryId = 1 });
            sut.Should().ContainEquivalentOf(new ZipCodeEntity { Zip = "5020", City = "Salzburg", CountryId = 1 });

            sut.Should().ContainEquivalentOf(new ZipCodeEntity { Zip = "84384", City = "Wittibreut", CountryId = 2 });
            sut.Should().ContainEquivalentOf(new ZipCodeEntity { Zip = "84359", City = "Simbach", CountryId = 2 });
        }
    }
}
