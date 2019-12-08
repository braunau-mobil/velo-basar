using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class StatisticContext : IStatisticContext
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IBasarContext _basarContext;
        private readonly IProductContext _productContext;
        private readonly ITransactionContext _transactionContext;

        public StatisticContext(IStringLocalizer<SharedResource> localizer, ITransactionContext transactionContext, IBasarContext basarContext, IProductContext productContext)
        {
            _localizer = localizer;
            _transactionContext = transactionContext;
            _basarContext = basarContext;
            _productContext = productContext;
        }

        public async Task<BasarStatistic> GetBasarStatisticAsnyc(int basarId)
        {
            var basar = await _basarContext.GetAsync(basarId);
            var products = await _productContext.GetProductsForBasar(basar).ToArrayAsync();

            //  @todo kinda hack, find better place to create the stats....
            return new BasarStatistic
            {
                Basar = basar,
                Sales = new PieChartData
                {
                    BackgroundColors = new[]
                    {
                        "'rgb(40, 167, 69)'",
                        "'rgb(255, 193, 7)'",
                        "'rgb(220, 53, 69)'",
                        "'rgb(220, 53, 69)'"
                    },
                    DataPoints = new[]
                    {
                        products.Count(x => x.StorageState == StorageState.Available),
                        products.Count(x => x.StorageState == StorageState.Sold),
                        products.Count(x => x.StorageState == StorageState.Gone),
                        products.Count(x => x.StorageState == StorageState.Locked)
                    },
                    Labels = new[]
                    {
                        _localizer["Verfügbar"].Value,
                        _localizer["Verkauft"].Value,
                        _localizer["Verschwunden"].Value,
                        _localizer["Gesperrt"].Value,
                    }
                }
            };
        }
        public async Task<SellerStatistics> GetSellerStatisticsAsync(Basar basar, int sellerId)
        {
            var products = await _productContext.GetProductsForSeller(basar, sellerId).ToArrayAsync();
            var soldProducts = products.Where(p => p.StorageState == StorageState.Sold).ToArray();
            return new SellerStatistics
            {
                AceptedProductCount = products.Length,
                SettlementAmout = soldProducts.Sum(p => p.Price),
                NotSoldProductCount = products.Where(p => p.StorageState != StorageState.Sold).Count(),
                PickedUpProductCount = products.Where(p => p.StorageState == StorageState.Gone && p.ValueState == ValueState.Settled).Count(),
                SoldProductCount = soldProducts.Length
            };
        }
        public async Task<TransactionStatistic[]> GetTransactionStatistics(TransactionType transactionType, Basar basar, int sellerId)
        {
            return await _transactionContext.GetMany(transactionType, basar, sellerId).AsNoTracking().Select(a => new TransactionStatistic
            {
                Transaction = a,
                ProductCount = a.Products.Count,
                Amount = a.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }
    }
}
