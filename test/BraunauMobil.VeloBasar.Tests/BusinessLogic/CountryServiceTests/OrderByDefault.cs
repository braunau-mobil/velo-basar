using Xan.AspNetCore.Mvc.Crud;
using Xan.AspNetCore.Parameter;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.CountryServiceTests;

public class OrderByDefault
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldOrderByName(CountryEntity[] countries)
    {
        //  Arrange
        Db.Countries.AddRange(countries);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<CountryEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null });

        //  Assert
        result.TotalItemCount.Should().Be(countries.Length);
        result.Should().BeInAscendingOrder(c => c.Entity.Name);
    }
}
