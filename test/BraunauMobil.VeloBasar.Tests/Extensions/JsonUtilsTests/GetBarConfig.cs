using BraunauMobil.VeloBasar.Extensions;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.Extensions.JsonUtilsTests
{
    public class GetBarConfig
    {
        [Fact]
        public void NoChartDataPoints_ShouldReturnEmptyJson()
        {
            //  Arrange
            ChartDataPoint[] points = Array.Empty<ChartDataPoint>();

            //  Act
            string result = JsonUtils.GetBarConfig(points, "Label", true);

            //  Assert
            result.Should().Be("""{"type":"bar","data":{"labels":[],"datasets":[{"label":"Label","fill":false,"data":[],"backgroundColor":[],"borderColor":[],"showLine":true}]}}""");
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
            string result = JsonUtils.GetBarConfig(points, "Label", true);

            //  Assert
            result.Should().Be("""{"type":"bar","data":{"labels":["Label1","Label2"],"datasets":[{"label":"Label","fill":false,"data":[1.0,2.0],"backgroundColor":["rgb(22, 33, 44)","rgb(66, 77, 89)"],"borderColor":["rgb(22, 33, 44)","rgb(66, 77, 89)"],"showLine":true}]}}""");
        }
    }
}
