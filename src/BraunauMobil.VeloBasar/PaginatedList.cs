using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList()
        {
            PageIndex = 0;
            TotalPages = 0;
        }

        public PaginatedList(List<T> items, int totalPages, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;

            AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var count = await source.CountAsync();
            var totalPages = CalcTotalPages(count, pageSize);
            pageIndex = Math.Min(pageIndex, totalPages);
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, totalPages, pageIndex, pageSize);
        }

        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
