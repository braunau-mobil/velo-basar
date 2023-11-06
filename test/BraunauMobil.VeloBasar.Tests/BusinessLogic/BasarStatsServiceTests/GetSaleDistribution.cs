using System.Collections.Generic;
using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public sealed class GetSaleDistribution
    : TestBase<EmptySqliteDbFixture>
{
    private readonly CultureInfo _initialCultureInfo = CultureInfo.CurrentCulture;
    private readonly Fixture _fixture = new();

    public GetSaleDistribution()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-AT");
    }

    public override void Dispose()
    {
        base.Dispose();

        CultureInfo.CurrentCulture = _initialCultureInfo;
    }

    [Fact]
    public void EmptyList_ShouldReturnEmpty()
    {
        //  Arrange
        IEnumerable<Tuple<TimeOnly, decimal>> transactionsAndTotals = Enumerable.Empty<Tuple<TimeOnly, decimal>>();

        //  Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetSaleDistribution(transactionsAndTotals);

        //  Assert
        result.Should().BeEmpty();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void ShouldGroupByHourAndMinuteAndSumValues(Color primaryColor)
    {
        //  Arrange
        ColorProvider.Setup(_ => _.Primary)
            .Returns(primaryColor);
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
        result.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(40, "11:22", primaryColor),
            new ChartDataPoint(170, "23:44", primaryColor)
        });

        ColorProvider.Verify(_ => _.Primary, Times.Exactly(2));
        VerifyNoOtherCalls();
    }

    private Tuple<TimeOnly, decimal> CreateDataPoint(int hour, int minute, decimal amount)
    {
        TimeOnly timeStamp = _fixture.Create<TimeOnly>();
        timeStamp = new TimeOnly(hour, minute, timeStamp.Second, timeStamp.Millisecond);

        return Tuple.Create(timeStamp, amount);
    }
}
