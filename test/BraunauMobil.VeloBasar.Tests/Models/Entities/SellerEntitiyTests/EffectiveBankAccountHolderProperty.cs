namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntitiyTests;

public class EffectiveBankAccountHolderProperty
{
    [Theory]
    [VeloAutoData]
    public void ShouldReturnFirstNameAndLastNameIfBankAccountHolderIsNull(SellerEntity sut)
    {
        //  Arrange
        sut.BankAccountHolder = null;
        sut.FirstName = "Frodo";
        sut.LastName = "Beutlin";

        //  Act
        string result = sut.EffectiveBankAccountHolder;

        //  Assert
        result.Should().Be("Frodo Beutlin");
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnBankAccountHolderIfNotNull(SellerEntity sut)
    {
        //  Arrange

        //  Act
        string result = sut.EffectiveBankAccountHolder;

        //  Assert
        result.Should().Be(sut.BankAccountHolder);
    }
}
