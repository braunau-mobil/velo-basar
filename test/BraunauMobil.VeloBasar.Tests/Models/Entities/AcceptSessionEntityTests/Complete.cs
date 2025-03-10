﻿namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AcceptSessionEntityTests;

public class Complete
{
    [Theory]
    [VeloAutoData]
    public void ShouldSetStates(AcceptSessionEntity sut, DateTime endTimeStamp)
    {
        //  Arrange
        sut.EndTimeStamp = null;
        sut.State = AcceptSessionState.Uncompleted;

        //  Act
        sut.Complete(endTimeStamp);

        //  Assert
        sut.EndTimeStamp.Should().Be(endTimeStamp);
        sut.State.Should().Be(AcceptSessionState.Completed);
    }
}
