namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class SanitizeIBAN
{
    [Theory]
    [VeloAutoData]
    public void ShouldDoNothingIfIBANIsNull(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = null;

        //  Act
        sut.SanitizeIBAN();

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
        sut.SanitizeIBAN();

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
        sut.SanitizeIBAN();

        //  Assert
        sut.IBAN.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldTrimWhitespacesAndOtherStuff(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "  AT12 3456-789012345678  ";

        //  Act
        sut.SanitizeIBAN();

        //  Assert
        sut.IBAN.Should().Be("AT123456789012345678");
    }

    [Theory]
    [VeloAutoData]
    public void ShouldMakeAllUppercase(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "at123456789012345678";

        //  Act
        sut.SanitizeIBAN();

        //  Assert
        sut.IBAN.Should().Be("AT123456789012345678");
    }

    [Theory]
    [VeloAutoData]
    public void AllTogether(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "  aT12 3456-789{012-3456$#78  ";

        //  Act
        sut.SanitizeIBAN();

        //  Assert
        sut.IBAN.Should().Be("AT123456789012345678");
    }
}
