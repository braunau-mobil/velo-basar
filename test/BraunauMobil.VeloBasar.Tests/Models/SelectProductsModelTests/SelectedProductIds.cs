using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.SelectProductsModelTests;

public class SelectedProductIds
{
    private readonly SelectProductsModel _sut = new();

    [Fact]
    public void Empy_ShouldReturnEmpty()
    {
        //  Arrange

        //  Act
        IEnumerable<int> result = _sut.SelectedProductIds();

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void NothingSelected_ShouldReturnEmpty(List<SelectModel<ProductEntity>> selectModels)
    {
        //  Arrange
        _sut.Products = selectModels;
        foreach (SelectModel<ProductEntity> selectModel in _sut.Products)
        {
            selectModel.IsSelected = false;
        }

        //  Act
        IEnumerable<int> result = _sut.SelectedProductIds();

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void SomeSelected_ShouldReturnOnlySelected(List<SelectModel<ProductEntity>> selected, List<SelectModel<ProductEntity>> unselected)
    {
        //  Arrange
        _sut.Products.Clear();
        foreach (SelectModel<ProductEntity> selectModel in selected)
        {
            selectModel.IsSelected = true;
        }
        foreach (SelectModel<ProductEntity> selectModel in unselected)
        {
            selectModel.IsSelected = false;
        }
        _sut.Products.AddRange(selected);
        _sut.Products.AddRange(unselected);

        //  Act
        IEnumerable<int> result = _sut.SelectedProductIds();

        //  Assert
        result.Should().BeEquivalentTo(selected.Select(selectModel => selectModel.Value.Id));
    }
}
