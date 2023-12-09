namespace BraunauMobil.VeloBasar.Tests.Models.SellerGroupSettlementStatusTests;

public class Percentage
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(25, 0, 0)]
    [InlineData(45, 23, 51)]
    [InlineData(56, 56, 100)]
    public void ReturnsPercentage(int sellerCount, int settledSellerCount, int expectedPercentage)
    {
        //  Arrange
        SellerGroupSettlementStatus sut = new (sellerCount, settledSellerCount);

        //  Act
        int actualPercentage = sut.Percentage;

        //  Assert
        Assert.Equal(expectedPercentage, actualPercentage);
    }
}
