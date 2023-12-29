using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests;

public static class Helpers
{
    public static IPaginatedList<T> EmptyPaginatedList<T>()
        => new PaginatedList<T>(new List<T>(), 0, 0, 0, 0);
}
