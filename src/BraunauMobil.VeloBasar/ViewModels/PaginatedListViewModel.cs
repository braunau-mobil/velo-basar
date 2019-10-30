using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class PaginatedListViewModel<T> : ListViewModel<T>, IPaginatable
    {
        private readonly Func<int, VeloPage> _getPaginationPage;

        public PaginatedListViewModel() { }
        public PaginatedListViewModel(Basar basar, IList<T> items, ListCommand<T>[] commands, Func<int, VeloPage> getPaginationPage) : base(basar, items, commands)
        {
            _getPaginationPage = getPaginationPage;
        }

        public bool HasPreviousPage
        {
            get => PageIndex > 1;
        }
        public bool HasNextPage
        {
            get => PageIndex < TotalPages;
        }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public VeloPage GetPaginationPage(int pageIndex) => _getPaginationPage(pageIndex);

        public static async Task<PaginatedListViewModel<T>> CreateAsync(Basar basar, IQueryable<T> source, int pageIndex, int pageSize, Func<int, VeloPage> getPaginationPage, ListCommand<T>[] commands = null)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var count = await source.CountAsync();
            var totalPages = CalcTotalPages(count, pageSize);
            pageIndex = Math.Min(pageIndex, totalPages);
            var skipCount = Math.Max(pageIndex - 1, 0) * pageSize;
            var items = await source.Skip(skipCount).Take(pageSize).ToListAsync();

            return new PaginatedListViewModel<T>(basar, items, commands, getPaginationPage)
            {
                PageIndex = pageIndex,
                TotalPages = totalPages
            };
        }
        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
