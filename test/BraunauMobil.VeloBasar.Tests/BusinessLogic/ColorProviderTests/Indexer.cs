using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ColorProviderTests;

public class Indexer
{
    private static readonly string[] _colors = ["#007bff", "#6c757d", "#28a745"];

    [Theory]
    [VeloAutoData]
    public void ReturnsAlwaysTheSame(string value)
    {
        //  Arrange
        ColorProvider sut = CreateSut();

        //  Act
        Color c1 = sut[value];
        Color c2 = sut[value];

        //  Assert
        c1.Should().Be(c2);
    }

    [Theory]
    [VeloAutoData]
    public void ReturnsColorsInOrderForDifferentKeys(string key1, string key2, string key3)
    {
        //  Arrange
        ColorProvider sut = CreateSut();
        Color.TryParse(_colors[0], out Color expected1);
        Color.TryParse(_colors[1], out Color expected2);
        Color.TryParse(_colors[2], out Color expected3);

        //  Act
        Color c1 = sut[key1];
        Color c2 = sut[key2];
        Color c3 = sut[key3];

        //  Assert
        c1.Should().Be(expected1);
        c2.Should().Be(expected2);
        c3.Should().Be(expected3);
    }

    [Theory]
    [VeloAutoData]
    public void WrapsAroundWhenAllColorsAreUsed(string key1, string key2, string key3, string key4)
    {
        //  Arrange
        ColorProvider sut = CreateSut();
        Color.TryParse(_colors[0], out Color expected);

        //  Act
        _ = sut[key1];
        _ = sut[key2];
        _ = sut[key3];
        Color c4 = sut[key4];

        //  Assert
        c4.Should().Be(expected);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenKeyIsNull()
    {
        //  Arrange
        ColorProvider sut = CreateSut();

        //  Act
        Action act = () => _ = sut[null!];

        //  Assert
        act.Should().Throw<ArgumentNullException>();
    }

    private static ColorProvider CreateSut()
    {
        ApplicationSettings settings = new()
        {
            Colors = _colors
        };
        return new ColorProvider(new OptionsWrapper<ApplicationSettings>(settings));
    }
}
