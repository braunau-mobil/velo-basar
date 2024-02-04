using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public sealed class GetSaleDistribution
    : TestBase<EmptySqliteDbFixture>
{
    private readonly VeloFixture _fixture = new();

    [Fact]
    public void EmptyList_ShouldReturnEmpty()
    {
        //  Arrange
        IEnumerable<Tuple<TimeOnly, decimal>> transactionsAndTotals = Enumerable.Empty<Tuple<TimeOnly, decimal>>();

        //  Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetSaleDistribution(transactionsAndTotals);

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldGroupByHourAndMinuteAndSumValues(Color primaryColor)
    {
        //  Arrange
        A.CallTo(() => ColorProvider.Primary).Returns(primaryColor);
        var transactionsAndTotals = new []
        {
            CreateDataPoint(11, 22, 15),
            CreateDataPoint(23, 44, 60),
            CreateDataPoint(11, 22, 25),
            CreateDataPoint(23, 44, 10),
            CreateDataPoint(23, 44, 100),
        };

        //  Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetSaleDistribution(transactionsAndTotals);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(40, "11:22", primaryColor),
                new ChartDataPoint(170, "23:44", primaryColor)
            });
        }

        A.CallTo(() => ColorProvider.Primary).MustHaveHappenedTwiceExactly();
    }

    private Tuple<TimeOnly, decimal> CreateDataPoint(int hour, int minute, decimal amount)
    {
        TimeOnly timeStamp = _fixture.Create<TimeOnly>();
        timeStamp = new TimeOnly(hour, minute, timeStamp.Second, timeStamp.Millisecond);

        return Tuple.Create(timeStamp, amount);
    }
}
