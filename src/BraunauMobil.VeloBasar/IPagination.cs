using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public interface IPagination
    {
        int PageIndex { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get;  }
        string Page { get; }
        string CurrentFilter { get; }
        
        IDictionary<string, string> GetPaginationRoute();
    }
}
