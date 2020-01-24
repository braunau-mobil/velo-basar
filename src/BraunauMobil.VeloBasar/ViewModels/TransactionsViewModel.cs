﻿using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionsViewModel : IPaginatable
    {
        private readonly Func<int, VeloPage> _getPaginationPage;

        private TransactionsViewModel(IReadOnlyCollection<ProductsTransaction> transactions, IReadOnlyCollection<ListCommand<TransactionViewModel>> commands) : this(transactions, commands, null, 0, 0) { }
        private TransactionsViewModel(IReadOnlyCollection<ProductsTransaction> transactions, IReadOnlyCollection<ListCommand<TransactionViewModel>> commands, Func<int, VeloPage> getPaginationPage, int pageIndex, int totalPages)
        {
            ViewModels = transactions.Select(p => new TransactionViewModel(this, p)).ToArray();
            Commands = commands ?? Array.Empty<ListCommand<TransactionViewModel>>();
            _getPaginationPage = getPaginationPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }

        public bool ShowDocumentLink { get; set; }
        public bool ShowNotes { get; set; }
        public bool ShowType { get; set; }
        public bool NotEmpty { get => ViewModels.Any(); }
        public IReadOnlyList<TransactionViewModel> ViewModels { get; }
        public IReadOnlyCollection<ListCommand<TransactionViewModel>> Commands { get; }

        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get => PageIndex > 1; }
        public bool HasNextPage { get => PageIndex < TotalPages; }

        public VeloPage GetPaginationPage(int pageIndex) => _getPaginationPage(pageIndex);

        public static async Task<TransactionsViewModel> CreateAsync(IQueryable<ProductsTransaction> transactionsIq) => await CreateAsync(transactionsIq, null, null);
        public static async Task<TransactionsViewModel> CreateAsync(IQueryable<ProductsTransaction> transactionsIq, Func<TransactionViewModel, Task> decorateAsync, IReadOnlyCollection<ListCommand<TransactionViewModel>> commands)
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
        public static async Task<TransactionsViewModel> CreateAsync(IQueryable<ProductsTransaction> transactionsIq, int pageIndex, int pageSize, Func<int, VeloPage> getPaginationPage, IReadOnlyCollection<ListCommand<TransactionViewModel>> commands)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var count = await transactionsIq.CountAsync();
            var totalPages = CalcTotalPages(count, pageSize);
            pageIndex = Math.Min(pageIndex, totalPages);
            var skipCount = Math.Max(pageIndex - 1, 0) * pageSize;
            var items = await transactionsIq.Skip(skipCount).Take(pageSize).AsNoTracking().ToArrayAsync();

            return new TransactionsViewModel(items, commands, getPaginationPage, pageIndex, totalPages);
        }
        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
