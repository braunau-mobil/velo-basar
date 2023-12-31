using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class ValueStates
    : TestBase
{
    private readonly List<Action<SelectListItem>> _elementInspectors;

    public ValueStates()
    {
        _elementInspectors = new List<Action<SelectListItem>>()
        {
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_NotSettled");
                item.Value.Should().Be("NotSettled");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Settled");
                item.Value.Should().Be("Settled");
            }
        };
    }

    [Fact]
    public void DoNotIncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.ValueStates();

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }

    [Fact]
    public void IncludeAll()
    {
        //  Arrange
        _elementInspectors.Insert(0, item =>
        {
            item.Disabled.Should().BeFalse();
            item.Group.Should().BeNull();
            item.Selected.Should().BeFalse();
            item.Text.Should().Be("VeloBasar_AllValueStates");
            item.Value.Should().Be("");
        });

        // Act
        SelectList result = Sut.ValueStates(includeAll: true);

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }
}
