using BraunauMobil.VeloBasar.BusinessLogic;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ColorProviderTests;

public class Indexer
{
    [Theory]
    [AutoData]
    public void ReturnsAlwaysTheSame(string value)
    {
        //  Arrange
        ColorProvider sut = new();

        //  Act
        Color c1 = sut[value];
        Color c2 = sut[value];

        //  Assert
        c1.Should().Be(c2);
    }
}
