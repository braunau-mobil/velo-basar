using Microsoft.AspNetCore.Mvc.Rendering;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class CountriesAsync
    : TestBase
{
    private readonly Fixture _fixture = new();
    private readonly List<Action<SelectListItem>> _elementInspectors = new();

    [Fact]
    public async Task Default_ShouldReturnEnabledCountries()
    {
        //  Arrange
        await InsertTestData();

        //  Act
        SelectList result = await Sut.CountriesAsync();

        //  Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }

    private async Task InsertTestData()
    {
        CountryEntity country1 = CreateCountryType("country1", ObjectState.Enabled);
        CountryEntity country2 = CreateCountryType("country2", ObjectState.Disabled);
        CountryEntity country3 = CreateCountryType("country3", ObjectState.Enabled);
        CountryEntity country4 = CreateCountryType("country4", ObjectState.Enabled);
        CountryEntity country5 = CreateCountryType("country5", ObjectState.Disabled);
        CountryEntity country6 = CreateCountryType("country6", ObjectState.Disabled);

        Db.Countries.AddRange(country1, country2, country3, country4, country5, country6);
        await Db.SaveChangesAsync();

        AddInspector(country1);
        AddInspector(country3);
        AddInspector(country4);
    }

    private void AddInspector(CountryEntity country)
    {
        void inspector(SelectListItem item)
        {
            item.Disabled.Should().BeFalse();
            item.Group.Should().BeNull();
            item.Selected.Should().BeFalse();
            item.Text.Should().Be(country.Name);
            item.Value.Should().Be(country.Id.ToString());
        }
        _elementInspectors.Add(inspector);
    }

    private CountryEntity CreateCountryType(string isoCode, ObjectState state)
    {
        CountryEntity country = _fixture.Build<CountryEntity>()
            .With(_ => _.State, state)
            .With(_ => _.Iso3166Alpha3Code, isoCode)
            .Create();
        return country;
    }
}
