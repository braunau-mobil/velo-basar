using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BraunauMobil.VeloBasar;

public sealed class VeloBasarAppContext
    : IAppContext
{
    private readonly IWebHostEnvironment _env;
    private readonly VeloDbContext _db;

    public VeloBasarAppContext(IWebHostEnvironment env, VeloDbContext db)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _db = db ?? throw new ArgumentNullException(nameof(db));

        Version? version = typeof(VeloBasarAppContext).Assembly.GetName().Version;
        if (version == null)
        {
            throw new InvalidOperationException("Could not read Assembly Version");
        }
        Version = version.ToString();
    }

    public string Version { get; }

    public bool DevToolsEnabled() => _env.IsDevelopment();

    public async Task<bool> NeedsInitialSetupAsync() => await _db.NeedsInitialSetupAsync();

    public async Task<bool> NeedsMigrationAsync() => await _db.NeedsMigrationAsync();

}
