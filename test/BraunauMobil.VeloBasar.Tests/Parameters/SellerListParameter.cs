using BraunauMobil.VeloBasar.Parameters;

namespace BraunauMobil.VeloBasar.Tests.Parameters;

public class SellerListParameterTests
{
    [Theory]
    [VeloAutoData]
    public void CopyCtor(SellerListParameter source)
    {
        //  Arrange

        //  Act
        SellerListParameter target = new (source);

        //  Assert
        target.Should().BeEquivalentTo(source);
    }
}
