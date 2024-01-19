using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class ProductRouterTests
{
    private readonly ProductRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToDetails()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDetails(666);

        //  Assert
        actual.Should().Be("//id=666&action=Details&controller=Product");
    }

    [Fact]
    public void ToEdit()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToEdit(123);

        //  Assert
        actual.Should().Be("//id=123&action=Edit&controller=Product");
    }

    [Fact]
    public void ToLabel()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToLabel(678);

        //  Assert
        actual.Should().Be("//id=678&action=Label&controller=Product");
    }

    [Fact]
    public void ToList()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList();

        //  Assert
        actual.Should().Be("//BasarId=0&PageIndex=1&State=Enabled&action=List&controller=Product");
    }

    [Fact]
    public void ToList_PageIndex()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList(33);

        //  Assert
        actual.Should().Be("//BasarId=0&PageIndex=33&State=Enabled&action=List&controller=Product");
    }


    [Theory]
    [InlineData(null, 321, "//BasarId=0&PageIndex=321&State=Enabled&action=List&controller=Product")]
    [InlineData(456, 123, "//BasarId=0&PageIndex=123&PageSize=456&State=Enabled&action=List&controller=Product")]
    public void ToList_PageSize_PageIndex(int? pageSize, int pageIndex, string expectedResult)
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList(pageSize, pageIndex);

        //  Assert
        actual.Should().Be(expectedResult);
    }


    [Theory]
    [InlineData(679, null, "", null, "//SearchString=&PageIndex=679&action=List&controller=Product")]
    [InlineData(456, 3489, "search", ObjectState.Disabled, "//SearchString=search&PageIndex=456&PageSize=3489&State=Disabled&action=List&controller=Product")]
    public void ToList_ListParameter(int pageIndex, int? pageSize, string searchString, ObjectState? state, string expectedResult)
    {
        //  Arrange
        ListParameter parameter = new()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchString = searchString,
            State = state
        };

        //  Act
        string actual = _sut.ToList(parameter);

        //  Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(888, null, 123, null, null, "", null, null, null, "//BasarId=888&SearchString=&PageIndex=123&action=List&controller=Product")]
    [InlineData(333, "myBrand", 123, 777, 864, "mySearch", ObjectState.Disabled, StorageState.Lost, ValueState.Settled, "//BasarId=333&StorageState=Lost&ValueState=Settled&Brand=myBrand&ProductTypeId=864&SearchString=mySearch&PageIndex=123&PageSize=777&State=Disabled&action=List&controller=Product")]
    public void ToList_ProductListParameter(int basarId, string? brand, int pageIndex, int? pageSize, int? productTypeId, string searchString, ObjectState? state, StorageState? storageState, ValueState? valueState, string expectedResult)
    {
        //  Arrange
        ProductListParameter parameter = new()
        {
            BasarId = basarId,
            Brand = brand,
            PageIndex = pageIndex,
            PageSize = pageSize,
            ProductTypeId = productTypeId,
            SearchString = searchString,
            State = state,
            StorageState = storageState,
            ValueState = valueState            
        };

        //  Act
        string actual = _sut.ToList(parameter);

        //  Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public void ToLock()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToLock(3333);

        //  Assert
        actual.Should().Be("//id=3333&action=Lock&controller=Product");
    }

    [Fact]
    public void ToSetAsLost()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSetAsLost(1234);

        //  Assert
        actual.Should().Be("//id=1234&action=Lost&controller=Product");
    }

    [Fact]
    public void ToUnlock()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToUnlock(9876);

        //  Assert
        actual.Should().Be("//id=9876&action=UnLock&controller=Product");

    }
}
