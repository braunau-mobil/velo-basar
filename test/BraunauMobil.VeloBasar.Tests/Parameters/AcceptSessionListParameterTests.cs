using BraunauMobil.VeloBasar.Parameters;

namespace BraunauMobil.VeloBasar.Tests.Parameters;

public class AcceptSessionListParameterTests
{
    [Theory]
    [VeloAutoData]
    public void CopyCtor(AcceptSessionListParameter source)
    {
        //  Arrange

        //  Act
        AcceptSessionListParameter target = new (source);

        //  Assert
        target.Should().BeEquivalentTo(source);
    }
}
