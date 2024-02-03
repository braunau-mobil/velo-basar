using Xan.AspNetCore.Mvc.Crud;
using Xan.AspNetCore.Parameter;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductTypeServiceTests;

public class OrderByDefault
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldOrderByName(ProductTypeEntity[] productTypes)
    {
        //  Arrange
        Db.ProductTypes.AddRange(productTypes);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<ProductTypeEntity>> result = await Sut.GetManyAsync(new ListParameter() { PageSize = int.MaxValue, State = null });

        //  Assert
        result.TotalItemCount.Should().Be(productTypes.Length);
        result.Should().BeInAscendingOrder(c => c.Entity.Name);
    }
}
