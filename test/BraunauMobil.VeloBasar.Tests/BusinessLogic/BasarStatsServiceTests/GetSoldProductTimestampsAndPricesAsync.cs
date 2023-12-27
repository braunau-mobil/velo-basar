namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductTimestampsAndPricesAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task NoSalesAtAll_ShouldReturnEmpty(int basarId)
    {
        //  Arrange

        //  Act
        IReadOnlyList<Tuple<TimeOnly, decimal>> result = await Sut.GetSoldProductTimestampsAndPricesAsync(basarId);

        //  Assert
        result.Should().BeEmpty();
    }
}
