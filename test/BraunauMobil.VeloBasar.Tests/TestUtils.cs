using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests
{
    public static class TestUtils
    {
        public static IStringLocalizer<SharedResource> CreateLocalizer()
        {
            var factory = new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance);
            return new StringLocalizer<SharedResource>(factory);
        }
    }
}
