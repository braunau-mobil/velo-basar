namespace BraunauMobil.VeloBasar.Tests.Models.TransactionSuccessModelTests;

public class ShowChange
{
    [Theory]
    [VeloAutoData]
    public void ChangeAmountIsZero_ShouldBeFalse(TransactionSuccessModel sut)
    {
        //  Arrange
        sut.Entity.Change = new(0);

        //  Act

        //  Assert
        sut.ShowChange.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public void ChangeAmountIsNotZero_ShouldBeTrue(TransactionSuccessModel sut, decimal amount)
    {
        //  Arrange
        sut.Entity.Change = new(amount);

        //  Act

        //  Assert
        sut.ShowChange.Should().BeTrue();
    }
}
