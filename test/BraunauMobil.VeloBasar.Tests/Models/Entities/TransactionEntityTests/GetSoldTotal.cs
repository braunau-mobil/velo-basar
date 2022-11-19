namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class GetSoldTotal
{
    [Fact]
    public void NotProducts_Zero()
    {
        TransactionEntity sut = new();
        Assert.Equal(0.0m, sut.GetSoldTotal());
    }
}
