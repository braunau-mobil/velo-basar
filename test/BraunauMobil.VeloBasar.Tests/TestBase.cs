using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        protected static IStringLocalizer<SharedResource> GetLocalizer()
        {
            var factory = new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance);
            return new StringLocalizer<SharedResource>(factory);
        }
    }
}
