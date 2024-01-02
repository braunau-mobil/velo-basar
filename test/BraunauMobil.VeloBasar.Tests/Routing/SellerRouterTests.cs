using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class SellerRouterTests
{
    private readonly SellerRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToCreateForAcceptance()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCreateForAcceptance();

        //  Assert
        actual.Should().Be("//action=CreateForAcceptance&controller=Seller");
    }

    [Fact]
    public void ToCreateForAcceptance_WithId()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCreateForAcceptance(123);

        //  Assert
        actual.Should().Be("//id=123&action=CreateForAcceptance&controller=Seller");
    }

    [Fact]
    public void ToSearchForAcceptance()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSearchForAcceptance();

        //  Assert
        actual.Should().Be("//action=SearchForAcceptance&controller=Seller");
    }

    [Fact]
    public void ToDetails()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDetails(987);

        //  Assert
        actual.Should().Be("//id=987&action=Details&controller=Seller");
    }

    [Fact]
    public void ToLabels()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToLabels(567);

        //  Assert
        actual.Should().Be("//id=567&action=Labels&controller=Seller");
    }

    [Fact]
    public void ToSettle()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSettle(444);

        //  Assert
        actual.Should().Be("//id=444&action=Settle&controller=Seller");
    }

    [Fact]
    public void ToTriggerStatusPush()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToTriggerStatusPush(222);

        //  Assert
        actual.Should().Be("//id=222&action=TriggerStatusPush&controller=Seller");

    }
}
