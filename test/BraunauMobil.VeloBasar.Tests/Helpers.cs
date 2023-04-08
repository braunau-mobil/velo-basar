using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class Helpers
    {
        public static IStringLocalizer<SharedResources> CreateActualLocalizer()
        {
            IOptions<LocalizationOptions> options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            ResourceManagerStringLocalizerFactory factory = new (options, NullLoggerFactory.Instance);
            return new StringLocalizer<SharedResources>(factory);
        }
    }
}
