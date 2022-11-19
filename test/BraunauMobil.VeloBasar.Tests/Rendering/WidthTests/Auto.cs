#warning @todo XAN Tests
using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.WidthTests;

public class Auto
{
    [Fact]
    public void Test()
    {
        var sut = Width.Auto;
        Assert.Equal("width: auto;", sut.GetStyle());
    }
}
