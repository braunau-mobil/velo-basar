using BraunauMobil.VeloBasar.Extensions;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.Extensions.JsonUtilsTests
{
    public class GetDonutConfig
    {
        [Fact]
        public void NoChartDataPoints_ShouldReturnEmptyJson()
        {
            //  Arrange
            ChartDataPoint[] points = [];

            //  Act
            string result = JsonUtils.GetDonutConfig(points);

            //  Assert
            result.Should().Be("""{"type":"doughnut","data":{"datasets":[{"data":[],"backgroundColor":[]}],"labels":[]}}""");
        }

        [Fact]
        public void ChartDataPoints_ShouldReturnValidJson()
        {
            //  Arrange
            ChartDataPoint[] points =
            [
                new ChartDataPoint(1, "Label1", Color.FromArgb(11, 22, 33, 44)),
                new ChartDataPoint(2, "Label2", Color.FromArgb(55, 66, 77, 89)),
            ];

            //  Act
            string result = JsonUtils.GetDonutConfig(points);

            //  Assert
            result.Should().Be("""{"type":"doughnut","data":{"datasets":[{"data":[1.0,2.0],"backgroundColor":["rgb(22, 33, 44)","rgb(66, 77, 89)"]}],"labels":["Label1","Label2"]}}""");
        }
    }
}
