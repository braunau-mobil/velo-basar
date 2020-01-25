namespace BraunauMobil.VeloBasar
{
    public interface IPaginatable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get;  }

        VeloPage GetPaginationPage(int pageIndex, int? pageSize = null);
    }
}
