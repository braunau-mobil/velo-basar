using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ColorProviderTests;

public class Constructor
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenSettingsIsNull()
    {
        //  Arrange

        //  Act
        Action act = () => _ = new ColorProvider(null!);

        //  Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldSetPrimaryToFirstColorInSettings()
    {
        //  Arrange
        ApplicationSettings settings = new()
        {
            Colors = ["#007bff", "#6c757d"]
        };

        //  Act
        ColorProvider sut = new(new OptionsWrapper<ApplicationSettings>(settings));

        //  Assert
        Color.TryParse("#007bff", out Color expected).Should().BeTrue();
        sut.Primary.Should().Be(expected);
    }

    [Fact]
    public void ShouldThrowInvalidOperationExceptionWhenAColorCannotBeParsed()
    {
        //  Arrange
        ApplicationSettings settings = new()
        {
            Colors = ["#007bff", "not-a-color"]
        };

        //  Act
        Action act = () => _ = new ColorProvider(new OptionsWrapper<ApplicationSettings>(settings));

        //  Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
