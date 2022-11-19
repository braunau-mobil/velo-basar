#warning @todo XAN Tests

using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.WidthTests;

public class Percent
{
    [Theory]
    [InlineData("width: 0%;", 0)]
    [InlineData("width: 50%;", 50)]
    [InlineData("width: 100%;", 100)]
    [InlineData("width: 200%;", 200)]
    public void Test(string expectedStyle, int percentValue)
    {
        var sut = Width.Percent(percentValue);
        Assert.Equal(expectedStyle, sut.GetStyle());
    }
}
