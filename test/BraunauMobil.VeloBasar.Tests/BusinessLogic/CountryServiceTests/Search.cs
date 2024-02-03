using Xan.AspNetCore.Mvc.Crud;
using Xan.AspNetCore.Parameter;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.CountryServiceTests;

public class Search
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task SearchStrinIsParsable_ShouldMatchId(CountryEntity countryToLookFor, CountryEntity[] countries)
    {
        //  Arrange
        Db.Countries.Add(countryToLookFor);
        Db.Countries.AddRange(countries);
        await Db.SaveChangesAsync();

        string searchString = countryToLookFor.Id.ToString();

        //  Act
        IPaginatedList<CrudItemModel<CountryEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null, SearchString = searchString });

        //  Assert
        result.Should().ContainSingle(model => model.Entity == countryToLookFor);
    }

    [Theory]
    [VeloAutoData]
    public async Task ShouldSearchInName(CountryEntity countryToLookFor, CountryEntity[] countries)
    {
        //  Arrange
        Db.Countries.Add(countryToLookFor);
        Db.Countries.AddRange(countries);
        await Db.SaveChangesAsync();

        string searchString = countryToLookFor.Name.Substring(1, 3);

        //  Act
        IPaginatedList<CrudItemModel<CountryEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null, SearchString = searchString });

        //  Assert
        result.Should().ContainSingle(model => model.Entity == countryToLookFor);
    }

    [Theory]
    [VeloAutoData]
    public async Task ShouldSearchInIso3166Alpha3CodeCode(CountryEntity countryToLookFor, CountryEntity[] countries)
    {
        //  Arrange
        Db.Countries.Add(countryToLookFor);
        Db.Countries.AddRange(countries);
        await Db.SaveChangesAsync();

        string searchString = countryToLookFor.Iso3166Alpha3Code.Substring(1, 3);

        //  Act
        IPaginatedList<CrudItemModel<CountryEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null, SearchString = searchString });

        //  Assert
        result.Should().ContainSingle(model => model.Entity == countryToLookFor);
    }
}
