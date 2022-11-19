namespace BraunauMobil.VeloBasar.Tests.Models.ChangeInfoTests;

public class Ctor
{
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
        Assert.Equal(count500, changeInfo.Denomination[500]);
        Assert.Equal(count200, changeInfo.Denomination[200]);
        Assert.Equal(count100, changeInfo.Denomination[100]);
        Assert.Equal(count50, changeInfo.Denomination[50]);
        Assert.Equal(count20, changeInfo.Denomination[20]);
        Assert.Equal(count10, changeInfo.Denomination[10]);
        Assert.Equal(count5, changeInfo.Denomination[5]);
        Assert.Equal(count2, changeInfo.Denomination[2]);
        Assert.Equal(count1, changeInfo.Denomination[1]);
        Assert.Equal(count050, changeInfo.Denomination[0.5m]);
        Assert.Equal(count020, changeInfo.Denomination[0.2m]);
        Assert.Equal(count010, changeInfo.Denomination[0.1m]);
        Assert.Equal(count005, changeInfo.Denomination[0.05m]);
        Assert.Equal(count002, changeInfo.Denomination[0.02m]);
        Assert.Equal(count001, changeInfo.Denomination[0.01m]);
    }
}
