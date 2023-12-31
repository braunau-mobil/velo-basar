using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class BasarRouterTests
{
    private readonly BasarRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToActiveBasarDetails()
    {
        //  Arrange

        //  Act
        string result = _sut.ToActiveBasarDetails();

        //  Assert
        result.Should().Be("//action=ActiveBasarDetails&controller=Basar");
    }
    
    [Fact]
    public void ToDetails()
    {
        //  Arrange

        //  Act
        string result = _sut.ToDetails(854);

        //  Assert
        result.Should().Be("//id=854&action=Details&controller=Basar");
    }
}
