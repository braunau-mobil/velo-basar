using System.IO;
using Microsoft.Extensions.Caching.Memory;

namespace BraunauMobil.VeloBasar;

public interface IVeloCache
{
    /// <summary>
    /// True once the database has been found to be set up and fully migrated.
    /// Must be reset when the database is dropped from within the app.
    /// </summary>
    bool IsDatabaseReady { get; set; }

    Task<byte[]> GetBannerBytesAsync(string bannerFilePath);
}

public class VeloCache(IMemoryCache memoryCache)
    : IVeloCache
{
    private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    private volatile bool _isDatabaseReady;

    public bool IsDatabaseReady
    {
        get => _isDatabaseReady;
        set => _isDatabaseReady = value;
    }

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
