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
        public PaginatedListViewModel() { }
        public PaginatedListViewModel(Basar basar, IList<T> items, ListCommand<T>[] commands) : base(basar, items, commands)
        {
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
        public string PaginationPagePath { get; set; }
        public Func<IDictionary<string, string>> GetPaginationPageRouteFunc { get; set; }

        public IDictionary<string, string> GetPaginationRoute()
        {
            return GetPaginationPageRouteFunc();
        }

        public static async Task<PaginatedListViewModel<T>> CreateAsync(Basar basar, IQueryable<T> source, int pageIndex, int pageSize, string listPagePath, Func<IDictionary<string, string>> getListPageRoute, ListCommand<T>[] commands = null)
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

            return new PaginatedListViewModel<T>(basar, items, commands)
            {
                PageIndex = pageIndex,
                TotalPages = totalPages,
                PaginationPagePath = listPagePath,
                GetPaginationPageRouteFunc = getListPageRoute
            };
        }
        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }


    }
}
