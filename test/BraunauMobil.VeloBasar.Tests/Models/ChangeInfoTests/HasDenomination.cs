namespace BraunauMobil.VeloBasar.Tests.Models.ChangeInfoTests;

public class HasDenomination
{
    [Fact]
    public void NoAmount_ShouldReturnFalse()
    {
        //  Arrange
        ChangeInfo sut = new (0);

        //  Act
        bool result = sut.HasDenomination;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public void WithAmount_ShouldReturnTrue(decimal amount)
    {
        //  Arrange
        ChangeInfo sut = new(amount);

        //  Act
        bool result = sut.HasDenomination;

        //  Assert
        result.Should().BeTrue();
    }
}
