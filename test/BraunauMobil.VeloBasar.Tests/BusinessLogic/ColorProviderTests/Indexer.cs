using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ColorProviderTests;

public class Indexer
{
    [Theory]
    [VeloAutoData]
    public void ReturnsAlwaysTheSame(string value)
    {
        //  Arrange
        ColorProvider sut = new(new OptionsWrapper<ApplicationSettings>(new ApplicationSettings()));

        //  Act
        Color c1 = sut[value];
        Color c2 = sut[value];

        //  Assert
        c1.Should().Be(c2);
    }
}
