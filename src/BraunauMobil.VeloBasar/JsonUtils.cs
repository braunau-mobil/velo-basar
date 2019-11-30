using Newtonsoft.Json;
using System.Diagnostics.Contracts;
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
    }
}
