using BraunauMobil.VeloBasar.Models;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace BraunauMobil.VeloBasar
{
    public static class JsonUtils
    {
        public static T DeserializeFromJson<T>(byte[] data) where T : class
        {
            Contract.Requires(data != null);

            var json = Encoding.UTF8.GetString(data, 0, data.Length);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static byte[] SerializeAsJson<T>(this T instance) where T : class
        {
            var json = JsonConvert.SerializeObject(instance);
            return Encoding.UTF8.GetBytes(json);
        }
        public static string GetDonutConfig(PieChartDataPoint[] points)
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
                            backgroundColor = points.Select(p => $"rgb({p.Color.R}, {p.Color.G}, {p.Color.B})")
                        }
                    },
                    labels = points.Select(p => p.Label)
                }
            };
            return JsonConvert.SerializeObject(config);
        }
        public static string GetLineConfig(LineChartDataPoint[] points)
        {
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
                            fill = false,
                            data = points.Select(p => p.Value)
                        }
                    }
                }
            };
            return JsonConvert.SerializeObject(config);
        }
    }
}
