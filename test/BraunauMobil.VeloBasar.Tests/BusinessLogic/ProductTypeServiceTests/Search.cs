using Xan.AspNetCore.Mvc.Crud;
using Xan.AspNetCore.Parameter;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductTypeServiceTests;

public class Search
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task SearchStrinIsParsable_ShouldMatchId(ProductTypeEntity productTypeToLookFor, ProductTypeEntity[] productTypes)
    {
        //  Arrange
        Db.ProductTypes.Add(productTypeToLookFor);
        Db.ProductTypes.AddRange(productTypes);
        await Db.SaveChangesAsync();

        string searchString = productTypeToLookFor.Id.ToString();

        //  Act
        IPaginatedList<CrudItemModel<ProductTypeEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null, SearchString = searchString });

        //  Assert
        result.Should().ContainSingle(model => model.Entity == productTypeToLookFor);
    }

    [Theory]
    [VeloAutoData]
    public async Task ShouldSearchInName(ProductTypeEntity productTypeToLookFor, ProductTypeEntity[] productTypes)
    {
        //  Arrange
        Db.ProductTypes.Add(productTypeToLookFor);
        Db.ProductTypes.AddRange(productTypes);
        await Db.SaveChangesAsync();

        string searchString = productTypeToLookFor.Name.Substring(1, 3);

        //  Act
        IPaginatedList<CrudItemModel<ProductTypeEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null, SearchString = searchString });

        //  Assert
        result.Should().ContainSingle(model => model.Entity == productTypeToLookFor);
    }
}
