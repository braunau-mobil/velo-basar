using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;


public class AcceptStates
    : TestBase
{
    [Fact]
    public void DoNotIncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.AcceptStates();

        // Assert
        result.Should().SatisfyRespectively(
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Uncompleted");
                item.Value.Should().Be("Uncompleted");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Completed");
                item.Value.Should().Be("Completed");
            }
        );
    }

    [Fact]
    public void IncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.AcceptStates(includeAll: true);

        // Assert
        result.Should().SatisfyRespectively(
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_AllStates");
                item.Value.Should().Be("");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Uncompleted");
                item.Value.Should().Be("Uncompleted");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_Completed");
                item.Value.Should().Be("Completed");
            }
        );
    }
}
