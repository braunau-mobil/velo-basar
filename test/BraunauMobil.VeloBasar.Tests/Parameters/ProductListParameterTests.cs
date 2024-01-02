using BraunauMobil.VeloBasar.Parameters;

namespace BraunauMobil.VeloBasar.Tests.Parameters;

public class ProductListParameterTests
{
    [Theory]
    [VeloAutoData]
    public void CopyCtor(ProductListParameter source)
    {
        //  Arrange

        //  Act
        ProductListParameter target = new (source);

        //  Assert
        target.Should().BeEquivalentTo(source);
    }
}
