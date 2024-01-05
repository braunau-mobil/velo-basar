using Newtonsoft.Json;

using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Extensions;

public static class JsonUtils
{
    public static string GetDonutConfig(IReadOnlyList<ChartDataPoint> points)
    {
        ArgumentNullException.ThrowIfNull(points);

        var config = new
        {
            type = "doughnut",
            data = new
            {
                datasets = new[]
                {
                    new
                    {
                        data = points.Select(p => p.Value),
                        backgroundColor = points.Select(p => ToChartJsColor(p.Color))
                    }
                },
                labels = points.Select(p => p.Label)
            }
        };
        return JsonConvert.SerializeObject(config);
    }

    public static string GetLineConfig(IReadOnlyList<ChartDataPoint> points, string label, bool showLine)
    {
        ArgumentNullException.ThrowIfNull(points);
        ArgumentNullException.ThrowIfNull(label);

        string color = ToChartJsColor(new Color());
        if (points.Any())
        {
            color = ToChartJsColor(points[0].Color);
        }

        var config = new
        {
            type = "line",
            data = new
            {
                labels = points.Select(p => p.Label),
                datasets = new[]
                {
                    new
                    {
                        label,
                        fill = false,
                        data = points.Select(p => p.Value),
                        backgroundColor = color,
                        borderColor = color,
                        showLine
                    }
                }
            }
        };
        return JsonConvert.SerializeObject(config);
    }

    public static string ToChartJsColor(Color c)
        => $"rgb({c.R}, {c.G}, {c.B})";
}
