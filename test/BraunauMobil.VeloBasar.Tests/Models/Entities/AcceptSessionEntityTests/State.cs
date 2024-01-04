namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AcceptSessionEntityTests;

public class State
{
    [Fact]
    public void DefaultValueShouldBeUncompleted()
    {
        //  Arrange
        AcceptSessionEntity sut = new();

        //  Act
        AcceptSessionState state = sut.State;

        //  Assert
        state.Should().Be(AcceptSessionState.Uncompleted);
    }
}
