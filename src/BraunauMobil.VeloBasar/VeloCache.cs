using System.IO;
using Microsoft.Extensions.Caching.Memory;

namespace BraunauMobil.VeloBasar;

public interface IVeloCache
{
    Task<byte[]> GetBannerBytesAsync(string bannerFilePath);
}

public class VeloCache(IMemoryCache memoryCache)
    : IVeloCache
{
    private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

    public async Task<byte[]> GetBannerBytesAsync(string bannerFilePath)
    {
        ArgumentNullException.ThrowIfNull(bannerFilePath);

        string bannerKey = $"BannerFileBytes_{bannerFilePath}";
        if (_memoryCache.TryGetValue(bannerKey, out byte[]? data) && data is not null)
        {
            return data;
        }

        byte[] bannerBytes = await File.ReadAllBytesAsync(bannerFilePath);
        _memoryCache.Set(bannerKey, bannerBytes);
        return bannerBytes;
    }
}
