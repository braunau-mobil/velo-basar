using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class PaginatedListViewModel<T> : ListViewModel<T>, IPaginatable
    {
        private readonly Func<int, int?, VeloPage> _getPaginationPage;

        public PaginatedListViewModel() { }
        public PaginatedListViewModel(Basar basar, IReadOnlyList<T> items, ListCommand<T>[] commands, Func<int, int?, VeloPage> getPaginationPage, int pageIndex, int pageSize, int totalPages) : base(basar, items, commands)
        {
            _getPaginationPage = getPaginationPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            PageSize = pageSize;
        }

        public bool HasPreviousPage
        {
            get => PageIndex > 1;
        }
        public bool HasNextPage
        {
            get => PageIndex < TotalPages;
        }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }

        public VeloPage GetPaginationPage(int pageIndex, int? pageSize = null) => _getPaginationPage(pageIndex, pageSize);
    }
}
