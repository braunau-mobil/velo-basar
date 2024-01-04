using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AbstractCrudEntityTests;

public class State
{
    private class TestEntity
        : AbstractCrudEntity
    { }

    [Fact]
    public void DefaultValueIsEnabled()
    {
        //  Arrange
        TestEntity testEntity = new();

        //  Act
        ObjectState state = testEntity.State;

        //  Assert
        state.Should().Be(ObjectState.Enabled);
    }
}
