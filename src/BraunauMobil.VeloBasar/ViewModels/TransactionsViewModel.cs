using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionsViewModel : IPaginatable
    {
        private readonly Func<int, int?, VeloPage> _getPaginationPage;

        private TransactionsViewModel(IReadOnlyCollection<ProductsTransaction> transactions, IReadOnlyCollection<ListCommand<TransactionItemViewModel>> commands) : this(transactions, commands, null, 0, int.MaxValue, 0) { }
        private TransactionsViewModel(IReadOnlyCollection<ProductsTransaction> transactions, IReadOnlyCollection<ListCommand<TransactionItemViewModel>> commands, Func<int, int?, VeloPage> getPaginationPage, int pageIndex, int pageSize, int totalPages)
        {
            ViewModels = transactions.Select(p => new TransactionItemViewModel(this, p)).ToArray();
            Commands = commands ?? Array.Empty<ListCommand<TransactionItemViewModel>>();
            _getPaginationPage = getPaginationPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            PageSize = pageSize;
        }

        public bool ShowDocumentLink { get; set; }
        public bool ShowNotes { get; set; }
        public bool ShowType { get; set; }
        public bool NotEmpty { get => ViewModels.Any(); }
        public IReadOnlyList<TransactionItemViewModel> ViewModels { get; }
        public IReadOnlyCollection<ListCommand<TransactionItemViewModel>> Commands { get; }

        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get => PageIndex > 1; }
        public bool HasNextPage { get => PageIndex < TotalPages; }

        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => _getPaginationPage(pageIndex, pageSize);

        public static async Task<TransactionsViewModel> CreateAsync(IQueryable<ProductsTransaction> transactionsIq) => await CreateAsync(transactionsIq, null, null);
        public static async Task<TransactionsViewModel> CreateAsync(IQueryable<ProductsTransaction> transactionsIq, Func<TransactionItemViewModel, Task> decorateAsync, IReadOnlyCollection<ListCommand<TransactionItemViewModel>> commands)
        {
            var items = await transactionsIq.AsNoTracking().ToArrayAsync();

            var transactionsViewModel = new TransactionsViewModel(items, commands);
            if( decorateAsync != null)
            {
                foreach (var vm in transactionsViewModel.ViewModels)
                {
                    await decorateAsync(vm);
                }
            }
            return transactionsViewModel;
        }
        public static async Task<TransactionsViewModel> CreateAsync(IQueryable<ProductsTransaction> transactionsIq, int pageIndex, int pageSize, Func<int, int?, VeloPage> getPaginationPage, IReadOnlyCollection<ListCommand<TransactionItemViewModel>> commands)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            if (pageSize == int.MaxValue)
            {
                return new TransactionsViewModel(await transactionsIq.AsNoTracking().ToArrayAsync(), commands, getPaginationPage, pageIndex, pageSize, 1);
            }
            var count = await transactionsIq.CountAsync();
            var totalPages = CalcTotalPages(count, pageSize);
            pageIndex = Math.Min(pageIndex, totalPages);
            var skipCount = Math.Max(pageIndex - 1, 0) * pageSize;
            var items = await transactionsIq.Skip(skipCount).Take(pageSize).AsNoTracking().ToArrayAsync();

            return new TransactionsViewModel(items, commands, getPaginationPage, pageIndex, pageSize, totalPages);
        }
        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
