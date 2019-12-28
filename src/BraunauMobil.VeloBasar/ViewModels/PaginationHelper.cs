using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public static class PaginationHelper
    {
        public static async Task<PaginatedListViewModel<T>> CreateAsync<T>(Basar basar, IQueryable<T> source, int pageIndex, int pageSize, Func<int, VeloPage> getPaginationPage, ListCommand<T>[] commands = null)
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

            return new PaginatedListViewModel<T>(basar, items, commands, getPaginationPage, pageIndex, totalPages);
        }
        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
