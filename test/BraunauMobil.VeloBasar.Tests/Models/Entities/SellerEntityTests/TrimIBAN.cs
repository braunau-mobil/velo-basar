namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class TrimIBAN
{
    [Theory]
    [VeloAutoData]
    public void ShouldDoNothingIfIBANIsNull(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = null;

        //  Act
        sut.TrimIBAN();

        //  Assert
        sut.IBAN.Should().BeNull();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldDoNothingIfIBANIsEmpty(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "";

        //  Act
        sut.TrimIBAN();

        //  Assert
        sut.IBAN.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldTrimIfIBANIsWhitespace(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = " ";

        //  Act
        sut.TrimIBAN();

        //  Assert
        sut.IBAN.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldTrimIBAN(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "  AT123456789012345678  ";

        //  Act
        sut.TrimIBAN();

        //  Assert
        sut.IBAN.Should().Be("AT123456789012345678");
    }
}
