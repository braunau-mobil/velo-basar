using System.Text;
using System.Text.Json;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public static class JsonExtensionsHelpers
{
    public static T DeserializeFromJson<T>(this string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        T? deserialized = JsonSerializer.Deserialize<T>(json);
        deserialized.Should().NotBeNull();
        return deserialized!;
    }

    public static byte[] SerializeAsJson<T>(this T obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        string json = JsonSerializer.Serialize(obj);
        return Encoding.UTF8.GetBytes(json);
    }    
}
