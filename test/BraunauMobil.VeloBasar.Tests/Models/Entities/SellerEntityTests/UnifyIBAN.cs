namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class UnifyIBAN
{
    [Theory]
    [VeloAutoData]
    public void ShouldDoNothingIfIBANIsNull(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = null;

        //  Act
        sut.UnifyIBAN();

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
        sut.UnifyIBAN();

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
        sut.UnifyIBAN();

        //  Assert
        sut.IBAN.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldTrimWhitespaces(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "  AT123456789012345678  ";

        //  Act
        sut.UnifyIBAN();

        //  Assert
        sut.IBAN.Should().Be("AT123456789012345678");
    }

    [Theory]
    [VeloAutoData]
    public void ShouldMakeAlluppercase(SellerEntity sut)
    {
        //  Arrange
        sut.IBAN = "at123456789012345678";

        //  Act
        sut.UnifyIBAN();

        //  Assert
        sut.IBAN.Should().Be("AT123456789012345678");
    }
}
