using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class AdminRouterTests
{
    private readonly AdminRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToCreateSampleAcceptanceDocument()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCreateSampleAcceptanceDocument();

        //  Assert
        actual.Should().Be("//action=CreateSampleAcceptanceDocument&controller=Admin");
    }

    [Fact]
    public void ToCreateSampleLabels()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCreateSampleLabels();

        //  Assert
        actual.Should().Be("//action=CreateSampleLabels&controller=Admin");
    }

    [Fact]
    public void ToCreateSampleSaleDocument()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCreateSampleSaleDocument();

        //  Assert
        actual.Should().Be("//action=CreateSampleSaleDocument&controller=Admin");
    }

    [Fact]
    public void ToCreateSampleSettlementDocument()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCreateSampleSettlementDocument();

        //  Assert
        actual.Should().Be("//action=CreateSampleSettlementDocument&controller=Admin");
    }

    [Fact]
    public void ToExport()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToExport();

        //  Assert
        actual.Should().Be("//action=Export&controller=Admin");
    }

    [Fact]
    public void ToPrintTest()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToPrintTest();

        //  Assert
        actual.Should().Be("//action=PrintTest&controller=Admin");
    }
}
