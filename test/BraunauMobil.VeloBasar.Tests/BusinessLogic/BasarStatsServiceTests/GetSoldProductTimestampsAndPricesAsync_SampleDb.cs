namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductTimestampsAndPricesAsync_SampleDb
    : TestBase<SampleSqliteDbFixture>
{
    [Fact]
    public async Task BasarHasSales_ShouldReturnList()
    {
        //  Arrange

        //  Act
        IReadOnlyList<Tuple<TimeOnly, decimal>> result = await Sut.GetSoldProductTimestampsAndPricesAsync(1);

        //  Assert
        result.Should().HaveCount(72);
        result[0].Should().BeEquivalentTo((new TimeOnly(04, 37, 00), 69.43M));
        result[10].Should().BeEquivalentTo((new TimeOnly(05, 09, 00), 129.18M));
        result[45].Should().BeEquivalentTo((new TimeOnly(07, 28, 00), 90.03M));
        result[71].Should().BeEquivalentTo((new TimeOnly(08, 57, 00), 101.78M));

        VerifyNoOtherCalls();
    }
}
