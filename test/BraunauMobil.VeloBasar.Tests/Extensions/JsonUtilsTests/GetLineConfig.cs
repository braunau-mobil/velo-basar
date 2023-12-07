using BraunauMobil.VeloBasar.Extensions;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.Extensions.JsonUtilsTests
{
    public class GetLineConfig
    {
        [Fact]
        public void NoChartDataPoints_ShouldReturnEmptyJson()
        {
            //  Arrange
            ChartDataPoint[] points = Array.Empty<ChartDataPoint>();

            //  Act
            string result = JsonUtils.GetLineConfig(points, "Label");

            //  Assert
            result.Should().Be("""{"type":"line","data":{"labels":[],"datasets":[{"label":"Label","fill":false,"data":[],"backgroundColor":"rgb(0, 0, 0)","borderColor":"rgb(0, 0, 0)"}]}}""");
        }

        [Fact]
        public void ChartDataPoints_ShouldReturnValidJson()
        {
            //  Arrange
            ChartDataPoint[] points = new[]
            {
                new ChartDataPoint(1, "Label1", Color.FromArgb(11, 22, 33, 44)),
                new ChartDataPoint(2, "Label2", Color.FromArgb(55, 66, 77, 89)),
            };

            //  Act
            string result = JsonUtils.GetLineConfig(points, "Label");

            //  Assert
            result.Should().Be("""{"type":"line","data":{"labels":["Label1","Label2"],"datasets":[{"label":"Label","fill":false,"data":[1.0,2.0],"backgroundColor":"rgb(22, 33, 44)","borderColor":"rgb(22, 33, 44)"}]}}""");
        }
    }
}
