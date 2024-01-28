namespace BraunauMobil.VeloBasar.Tests.Models.ChangeInfoTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        ChangeInfo sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Amount.Should().Be(0);
            sut.Denomination.Should().BeEmpty();
            sut.HasDenomination.Should().BeFalse();
            sut.IsValid.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(87.99, 
        0, 0, 0, 
        1, 1, 1, 
        1, 1, 0, 
        1, 2, 0, 
        1, 2, 0)]
    public void OneProduct(decimal amount, 
        int count500, int count200, int count100, 
        int count50, int count20, int count10, 
        int count5, int count2, int count1, 
        int count050, int count020, int count010, 
        int count005, int count002, int count001)
    {
        ChangeInfo changeInfo = new (amount);
        using (new AssertionScope())
        {
            changeInfo.Denomination[500].Should().Be(count500);
            changeInfo.Denomination[200].Should().Be(count200);
            changeInfo.Denomination[100].Should().Be(count100);
            changeInfo.Denomination[50].Should().Be(count50);
            changeInfo.Denomination[20].Should().Be(count20);
            changeInfo.Denomination[10].Should().Be(count10);
            changeInfo.Denomination[5].Should().Be(count5);
            changeInfo.Denomination[2].Should().Be(count2);
            changeInfo.Denomination[1].Should().Be(count1);
            changeInfo.Denomination[0.5m].Should().Be(count050);
            changeInfo.Denomination[0.2m].Should().Be(count020);
            changeInfo.Denomination[0.1m].Should().Be(count010);
            changeInfo.Denomination[0.05m].Should().Be(count005);
            changeInfo.Denomination[0.02m].Should().Be(count002);
            changeInfo.Denomination[0.01m].Should().Be(count001);
        }
    }
}
