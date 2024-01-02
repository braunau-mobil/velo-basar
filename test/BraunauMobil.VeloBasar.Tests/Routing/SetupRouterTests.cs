using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class SetupRouterTests
{
    private readonly SetupRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToInitialSetup()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToInitialSetup();

        //  Assert
        actual.Should().Be("//action=InitialSetup&controller=Setup");
    }
}
