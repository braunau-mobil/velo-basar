namespace BraunauMobil.VeloBasar
{
    public interface IPaginatable
    {
        int PageIndex { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get;  }

        VeloPage GetPaginationPage(int pageIndex);
    }
}
