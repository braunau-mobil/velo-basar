﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class AcceptStates
    : TestBase
{
    private readonly List<Action<SelectListItem>> _elementInspectors;

    public AcceptStates()
    {
        _elementInspectors = new()
        {
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
        };
    }

    [Fact]
    public void DoNotIncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.AcceptStates();

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
            item.Text.Should().Be("VeloBasar_AllStates");
            item.Value.Should().Be("");
        });

        // Act
        SelectList result = Sut.AcceptStates(includeAll: true);

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }
}
