using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BraunauMobil.VeloBasar;

public sealed class VeloBasarAppContext
    : IAppContext
{
    private readonly IWebHostEnvironment _env;

    public VeloBasarAppContext(IWebHostEnvironment env)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));

        Version? version = typeof(VeloBasarAppContext).Assembly.GetName().Version;
        if (version == null)
        {
            throw new InvalidOperationException("Could not read Assembly Version");
        }
        Version = version.ToString();
    }

    public string Version { get; }

    public bool DevToolsEnabled() => _env.IsDevelopment();
}
