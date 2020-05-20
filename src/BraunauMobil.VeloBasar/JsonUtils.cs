using BraunauMobil.VeloBasar.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace BraunauMobil.VeloBasar
{
    public static class JsonUtils
    {
        public static T DeserializeFromJson<T>(byte[] data) where T : class
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var json = Encoding.UTF8.GetString(data, 0, data.Length);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static byte[] SerializeAsJson<T>(this T instance) where T : class
        {
            var json = JsonConvert.SerializeObject(instance);
            return Encoding.UTF8.GetBytes(json);
        }
        public static string GetDonutConfig(ChartDataPoint[] points)
        {
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
        public static string GetLineConfig(ChartDataPoint[] points, string label)
        {
            var color = ToChartJsColor(new Color());
            if (points.Any())
            {
                color = ToChartJsColor(points.FirstOrDefault().Color);
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
                            borderColor = color
                        }
                    }
                }
            };
            return JsonConvert.SerializeObject(config);
        }
        public static string ToChartJsColor(Color c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            return $"rgb({c.R}, {c.G}, {c.B})";
        }
    }
}
