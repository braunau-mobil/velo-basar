using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class StorageStates
    : TestBase
{
    private readonly List<Action<SelectListItem>> _elementInspectors;

    public StorageStates()
    {
        _elementInspectors = new List<Action<SelectListItem>>()
        {
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_NotAccepted");
                item.Value.Should().Be("NotAccepted");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Available");
                item.Value.Should().Be("Available");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Sold");
                item.Value.Should().Be("Sold");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Lost");
                item.Value.Should().Be("Lost");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Locked");
                item.Value.Should().Be("Locked");
            }
        };
    }

    [Fact]
    public void DoNotIncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.StorageStates();

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
            item.Text.Should().Be("VeloBasar_AllStorageStates");
            item.Value.Should().Be("");
        });

        // Act
        SelectList result = Sut.StorageStates(includeAll: true);

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }
}
