using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class TransactionRouterTests
{
    private readonly TransactionRouter _sut = new (new LinkGeneratorMock());

    [Fact]
    public void ToCancel()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCancel(222);

        //  Assert
        actual.Should().Be("//id=222&action=Cancel&controller=Transaction");
    }

    [Fact]
    public void ToDetails()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDetails(333);

        //  Assert
        actual.Should().Be("//id=333&action=Details&controller=Transaction");
    }

    [Fact]
    public void ToDocument()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDocument(444);

        //  Assert
        actual.Should().Be("//id=444&action=Document&controller=Transaction");
    }

    [Fact]
    public void ToList()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList();

        //  Assert
        actual.Should().Be("//BasarId=0&PageIndex=1&action=List&controller=Transaction");
    }


    [Theory]
    [InlineData(null, 123, "//BasarId=0&PageIndex=123&action=List&controller=Transaction")]
    [InlineData(444, 555, "//BasarId=0&PageIndex=555&PageSize=444&action=List&controller=Transaction")]
    public void ToList_PageSize_PageIndex(int? pageSize, int pageIndex, string expectedResult)
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList(pageSize, pageIndex);

        //  Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TransactionType.Acceptance, "//BasarId=0&TransactionType=Acceptance&PageIndex=1&action=List&controller=Transaction")]
    [InlineData(TransactionType.Sale, "//BasarId=0&TransactionType=Sale&PageIndex=1&action=List&controller=Transaction")]
    public void ToList_TransactionType(TransactionType transactionType, string expectedResult)
    {
        //  Arrange

        //  Act
        string actual = _sut.ToList(transactionType);

        //  Assert
        actual.Should().Be(expectedResult);
    }


    [Theory]
    [InlineData(777, null, "", null, "//SearchString=&PageIndex=777&action=List&controller=Transaction")]
    [InlineData(323, 456, "search", ObjectState.Disabled, "//SearchString=search&PageIndex=323&PageSize=456&State=Disabled&action=List&controller=Transaction")]
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
    [InlineData(333, 123, null, "", null, null, "//BasarId=333&SearchString=&PageIndex=123&action=List&controller=Transaction")]
    [InlineData(777, 321, 777, "mySearch", ObjectState.Disabled, TransactionType.Acceptance, "//BasarId=777&TransactionType=Acceptance&SearchString=mySearch&PageIndex=321&PageSize=777&State=Disabled&action=List&controller=Transaction")]
    public void ToList_TransactionListParameter(int basarId, int pageIndex, int? pageSize, string searchString, ObjectState? state, TransactionType? transactionType, string expectedResult)
    {
        //  Arrange
        TransactionListParameter parameter = new()
        {
            BasarId = basarId,
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchString = searchString,
            State = state,
            TransactionType = transactionType
        };

        //  Act
        string actual = _sut.ToList(parameter);

        //  Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public void ToSucess()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSucess(666);

        //  Assert
        actual.Should().Be("//id=666&action=Success&controller=Transaction");
    }
}
