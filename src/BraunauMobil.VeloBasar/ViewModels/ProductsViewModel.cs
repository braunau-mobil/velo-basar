using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ProductsViewModel : IPaginatable
    {
        private readonly Func<int, int?, VeloPage> _getPaginationPage;

        public ProductsViewModel(IEnumerable<ProductToTransaction> productToTransactions) : this(productToTransactions.GetProducts(), null) { }
        private ProductsViewModel(IReadOnlyCollection<Product> products, IReadOnlyCollection<ListCommand<ProductViewModel>> commands) : this(products, commands, null, 0, int.MaxValue, 0) { }
        private ProductsViewModel(IReadOnlyCollection<Product> products, IReadOnlyCollection<ListCommand<ProductViewModel>> commands, Func<int, int?, VeloPage> getPaginationPage, int pageIndex, int pageSize, int totalPages)
        {
            ViewModels = products.Select(p => new ProductViewModel(this, p)).ToArray();
            Commands = commands ?? Array.Empty<ListCommand<ProductViewModel>>();
            _getPaginationPage = getPaginationPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            PageSize = pageSize;
        }

        public bool ShowFooter { get; set; }
        public bool ShowSeller { get; set; }
        public decimal FooterValue { get; set; }
        public bool NotEmpty { get => ViewModels.Any(); }
        public IReadOnlyList<ProductViewModel> ViewModels { get; }
        public IReadOnlyCollection<ListCommand<ProductViewModel>> Commands { get; }

        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get => PageIndex > 1; }
        public bool HasNextPage { get => PageIndex < TotalPages; }

        public VeloPage GetPaginationPage(int pageIndex, int? pageSize = null) => _getPaginationPage(pageIndex, pageSize);

        public static async Task<ProductsViewModel> CreateAsync(IQueryable<Product> productsIq) => await CreateAsync(productsIq, null, null);
        public static async Task<ProductsViewModel> CreateAsync(IQueryable<Product> productsIq, Func<ProductViewModel, Task> decorateAsync, IReadOnlyCollection<ListCommand<ProductViewModel>> commands)
        {
            var items = await productsIq.AsNoTracking().ToArrayAsync();

            var productsViewModel = new ProductsViewModel(items, commands);
            if( decorateAsync != null)
            {
                foreach (var vm in productsViewModel.ViewModels)
                {
                    await decorateAsync(vm);
                }
            }
            return productsViewModel;
        }
        public static async Task<ProductsViewModel> CreateAsync(IQueryable<Product> productsIq, int pageIndex, int pageSize, Func<int, int?, VeloPage> getPaginationPage, IReadOnlyCollection<ListCommand<ProductViewModel>> commands)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            if (pageSize == int.MaxValue)
            {
                return new ProductsViewModel(await productsIq.AsNoTracking().ToArrayAsync(), commands, getPaginationPage, pageIndex, pageSize, 1);
            }
            var count = await productsIq.CountAsync();
            var totalPages = CalcTotalPages(count, pageSize);
            pageIndex = Math.Min(pageIndex, totalPages);
            var skipCount = Math.Max(pageIndex - 1, 0) * pageSize;
            var items = await productsIq.Skip(skipCount).Take(pageSize).AsNoTracking().ToArrayAsync();

            return new ProductsViewModel(items, commands, getPaginationPage, pageIndex, pageSize, totalPages);
        }
        private static int CalcTotalPages(int count, int pageSize)
        {
            return (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
