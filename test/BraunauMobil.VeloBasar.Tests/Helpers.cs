using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests;

public static class Helpers
{
    public static IStringLocalizer<SharedResources> CreateActualLocalizer()
    {
        IOptions<LocalizationOptions> options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        ResourceManagerStringLocalizerFactory factory = new (options, NullLoggerFactory.Instance);
        return new StringLocalizer<SharedResources>(factory);
    }

    public static IPaginatedList<T> EmptyPaginatedList<T>()
        => new PaginatedList<T>(new List<T>(), 0, 0, 0, 0);
}
