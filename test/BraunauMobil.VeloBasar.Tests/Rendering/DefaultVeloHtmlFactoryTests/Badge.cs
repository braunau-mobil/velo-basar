using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class Badge
    : TestBase
{
    [Fact]
    public void ShouldReturnCorrectHtml()
    {
        // Arrange

        // Act
        TagBuilder actual = Sut.Badge(VeloBasar.Rendering.BadgeType.Primary);

        // Assert
        actual.Should().Html("""<span class="badge rounded-pill text-bg-primary"></span>""");
    }
}
