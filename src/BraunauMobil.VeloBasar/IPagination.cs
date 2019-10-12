using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public interface IPagination : ISearchable
    {
        int PageIndex { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get;  }

        IDictionary<string, string> GetPaginationRoute();
    }
}
