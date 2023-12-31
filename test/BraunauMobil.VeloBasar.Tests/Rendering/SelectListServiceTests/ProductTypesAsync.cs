using Microsoft.AspNetCore.Mvc.Rendering;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class ProductTypesAsync
    : TestBase
{
    private readonly Fixture _fixture = new();
    private readonly List<Action<SelectListItem>> _elementInspectors = new();

    [Fact]
    public async Task Default_ShouldReturnEnabledProductTypes()
    {
        //  Arrange
        await InsertTestData();

        //  Act
        SelectList result = await Sut.ProductTypesAsync();

        //  Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }

    [Fact]
    public async Task IncludeAll_ShouldReturnEnabledProductTypes()
    {
        //  Arrange
        _elementInspectors.Add(item =>
        {
            item.Disabled.Should().BeFalse();
            item.Group.Should().BeNull();
            item.Selected.Should().BeFalse();
            item.Text.Should().Be("VeloBasar_AllProductTypes");
            item.Value.Should().Be("");
        });
        await InsertTestData();

        //  Act
        SelectList result = await Sut.ProductTypesAsync(includeAll: true);

        //  Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }

    private async Task InsertTestData()
    {
        ProductTypeEntity productType1 = CreateProductType("productType1", ObjectState.Enabled);
        ProductTypeEntity productType2 = CreateProductType("productType2", ObjectState.Disabled);
        ProductTypeEntity productType3 = CreateProductType("productType3", ObjectState.Enabled);
        ProductTypeEntity productType4 = CreateProductType("productType4", ObjectState.Enabled);
        ProductTypeEntity productType5 = CreateProductType("productType5", ObjectState.Disabled);
        ProductTypeEntity productType6 = CreateProductType("productType6", ObjectState.Disabled);

        Db.ProductTypes.AddRange(productType1, productType2, productType3, productType4, productType5, productType6);
        await Db.SaveChangesAsync();

        AddInspector(productType1);
        AddInspector(productType3);
        AddInspector(productType4);        
    }

    private void AddInspector(ProductTypeEntity productType)
    {
        void inspector(SelectListItem item)
        {
            item.Disabled.Should().BeFalse();
            item.Group.Should().BeNull();
            item.Selected.Should().BeFalse();
            item.Text.Should().Be(productType.Name);
            item.Value.Should().Be(productType.Id.ToString());
        }
        _elementInspectors.Add(inspector);
    }

    private ProductTypeEntity CreateProductType(string name, ObjectState state)
    {
        ProductTypeEntity productType = _fixture.Build<ProductTypeEntity>()
            .With(_ => _.State, state)
            .With(_ => _.Name, name)
            .Create();
        return productType;
    }
}
