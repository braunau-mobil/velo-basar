using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class PaginatedListViewModel<T> : ListViewModel<T>, IPaginatable
    {
        private readonly Func<int, VeloPage> _getPaginationPage;

        public PaginatedListViewModel() { }
        public PaginatedListViewModel(Basar basar, IReadOnlyList<T> items, ListCommand<T>[] commands, Func<int, VeloPage> getPaginationPage, int pageIndex, int totalPages) : base(basar, items, commands)
        {
            _getPaginationPage = getPaginationPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
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
        public int TotalPages { get; }

        public VeloPage GetPaginationPage(int pageIndex) => _getPaginationPage(pageIndex);
    }
}
