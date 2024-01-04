using BraunauMobil.VeloBasar.Tests;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AcceptSessionEntityTests;

public class IsCompleted
{
    [Theory]
    [VeloAutoData]
    public void NoEndTimestamp_ShouldNotBeCompleted(AcceptSessionEntity sut)
    {
        //  Arrange
        sut.EndTimeStamp = null;

        //  Act
        bool result = sut.IsCompleted;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public void WithEndTimestamp_ShouldNotBeCompleted(AcceptSessionEntity sut, DateTime endTimeStamp)
    {
        //  Arrange
        sut.EndTimeStamp = endTimeStamp;

        //  Act
        bool result = sut.IsCompleted;

        //  Assert
        result.Should().BeTrue();
    }
}
