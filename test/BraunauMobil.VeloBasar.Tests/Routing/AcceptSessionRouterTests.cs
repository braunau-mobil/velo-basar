using BraunauMobil.VeloBasar.Routing;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class AcceptSessionRouterTests
{
    private readonly AcceptSessionRouter _sut = new (new LinkGeneratorMock());

    [Fact]
    public void ToCancel_Default()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCancel(123);

        //  Assert
        actual.Should().Be("//sessionId=123&returnToList=False&action=Cancel&controller=AcceptSession");
    }

    [Fact]
    public void ToCancel_Default_ReturnToList()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCancel(321, true);

        //  Assert
        actual.Should().Be("//sessionId=321&returnToList=True&action=Cancel&controller=AcceptSession");
    }

    [Fact]
    public void ToList()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList();

        //  Assert
        actual.Should().Be("//PageIndex=1&State=Enabled&action=List&controller=AcceptSession");
    }

    [Fact]
    public void ToList_PageIndex()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList(66);

        //  Assert
        actual.Should().Be("//PageIndex=66&State=Enabled&action=List&controller=AcceptSession");
    }


    [Theory]
    [InlineData(null, 123, "//PageIndex=123&State=Enabled&action=List&controller=AcceptSession")]
    [InlineData(987, 321, "//PageIndex=321&PageSize=987&State=Enabled&action=List&controller=AcceptSession")]
    public void ToList_PageSize_PageIndex(int? pageSize, int pageIndex, string expectedResult)
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList(pageSize, pageIndex);

        //  Assert
        actual.Should().Be(expectedResult);
    }


    [Theory]
    [InlineData(123, null, "", null, "//SearchString=&PageIndex=123&action=List&controller=AcceptSession")]
    [InlineData(123, 987, "search", ObjectState.Disabled, "//SearchString=search&PageIndex=123&PageSize=987&State=Disabled&action=List&controller=AcceptSession")]
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

    [Fact]
    public void ToStart()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToStart();

        //  Assert
        actual.Should().Be("//action=Start&controller=AcceptSession");
    }

    [Fact]
    public void ToStartForSeller()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToStartForSeller(765);

        //  Assert
        actual.Should().Be("//sellerId=765&action=StartForSeller&controller=AcceptSession");
    }

    [Fact]
    public void ToSubmit()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSubmit(768);

        //  Assert
        actual.Should().Be("//sessionId=768&action=Submit&controller=AcceptSession");
    }

}
