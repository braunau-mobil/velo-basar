using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public sealed class GetSaleDistribution
    : TestBase
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
    public void ShouldGroupAndOrderByHourAndMinuteAndSumValues(Color primaryColor)
    {
        //  Arrange
        A.CallTo(() => ColorProvider.Primary).Returns(primaryColor);
        var transactionsAndTotals = new []
        {
            CreateDataPoint(23, 44, 21, 60),
            CreateDataPoint(11, 22, 01, 15),
            CreateDataPoint(11, 22, 11, 25),
            CreateDataPoint(23, 44, 44, 10),
            CreateDataPoint(23, 44, 21, 100),
            CreateDataPoint(08, 32, 21, 44.33M),
        };

        //  Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetSaleDistribution(transactionsAndTotals);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(44.33M, "08:32", primaryColor),
                new ChartDataPoint(40, "11:22", primaryColor),
                new ChartDataPoint(170, "23:44", primaryColor),
            });
        }

        A.CallTo(() => ColorProvider.Primary).MustHaveHappened(3, Times.Exactly);
    }

    private Tuple<TimeOnly, decimal> CreateDataPoint(int hour, int minute, int second, decimal amount)
    {
        TimeOnly timeStamp = _fixture.Create<TimeOnly>();
        timeStamp = new TimeOnly(hour, minute, second, timeStamp.Millisecond);

        return Tuple.Create(timeStamp, amount);
    }
}
