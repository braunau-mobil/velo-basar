using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class Alert
    : TestBase
{
    [Fact]
    public void TypeAndText_ShouldReturnCorrectHtml()
    {
        // Arrange

        // Act
        TagBuilder actual = Sut.Alert(MessageType.Danger, "text");

        // Assert
        actual.Should().BeHtml("""<div class="alert alert-danger" role="alert"><p>text</p></div>""");
    }

    [Fact]
    public void TypeAndTitleAndText_ShouldReturnCorrectHtml()
    {
        // Arrange

        // Act
        TagBuilder actual = Sut.Alert(MessageType.Danger, "title", "text");

        // Assert
        actual.Should().BeHtml("""<div class="alert alert-danger" role="alert"><h4 class="alert-heading">title</h4><p>text</p></div>""");
    }
}
