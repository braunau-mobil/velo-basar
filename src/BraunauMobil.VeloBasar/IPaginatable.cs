using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public interface IPaginatable
    {
        int PageIndex { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get;  }
        string PaginationPagePath { get; }

        IDictionary<string, string> GetPaginationRoute();
    }
}
