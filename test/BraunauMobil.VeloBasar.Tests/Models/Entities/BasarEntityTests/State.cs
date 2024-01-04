using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.BasarEntityTests;

public class State
{
    [Fact]
    public void DefaultValueShouldBeDisabled()
    {
        //  Arrange
        BasarEntity sut = new(); ;

        //  Act
        ObjectState result = sut.State;

        //  Assert
        result.Should().Be(ObjectState.Disabled);
    }
}
