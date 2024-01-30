namespace BraunauMobil.VeloBasar.Tests.Models.TransactionSuccessModelTests;

public class ShowAmountInput
{
    [Theory]
    [VeloAutoData]
    public void Sale_ShouldBeTrue(TransactionEntity sale)
    {
        //  Arrange
        sale.Type = TransactionType.Sale;

        //  Act
        TransactionSuccessModel sut = new(sale);

        //  Assert
        sut.ShowAmountInput.Should().BeTrue();
    }

    [Fact]
    public void NoSale_ShouldBeFalse()
    {
        //  Arrange
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(TransactionType.Sale);
        TransactionEntity sale = fixture.Create<TransactionEntity>();

        //  Act
        TransactionSuccessModel sut = new(sale);

        //  Assert
        sut.ShowAmountInput.Should().BeFalse();
    }
}
