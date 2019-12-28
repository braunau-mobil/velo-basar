using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
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
            var soldProducts = products.Where(p => p.StorageState == StorageState.Sold).ToArray();

            return new BasarStatistic
            {
                Basar = basar,
                AcceptedProductsCount = products.Length,
                AcceptedProductsAmount = products.SumPrice(),
                SoldProductsCount = soldProducts.Length,
                SoldProductsAmount = soldProducts.SumPrice(),
                AcceptedProductsByCount = GroupByProductType(products, FromCount),
                AcceptedProductsByAmount = GroupByProductType(products, FromAmount),
                SoldProductsByCount = GroupByProductType(soldProducts, FromCount),
                SoldProductsByAmount = GroupByProductType(soldProducts, FromAmount),
                PriceDistribution = GetPriceDistribution(products)
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

        private PieChartDataPoint[] GroupByProductType(IEnumerable<Product> products, Func<IGrouping<ProductType, Product>, PieChartDataPoint> getPoint)
        {
            var data = new List<PieChartDataPoint>();
            var colors = new ColorDispenser();
            foreach (var group in products.GroupBy(p => p.Type))
            {
                var point = getPoint(group);
                point.Color = colors.Next();
                data.Add(point);
            }
            return data.ToArray();
        }
        private static PieChartDataPoint FromCount(IGrouping<ProductType, Product> group)
        {
            var count = group.Count();
            return new PieChartDataPoint
            {
                Label = $"{group.Key.Name}: {count}",
                Value = count
            };
        }
        private static PieChartDataPoint FromAmount(IGrouping<ProductType, Product> group)
        {
            var sum = group.SumPrice();
            return new PieChartDataPoint
            {
                Label = $"{group.Key.Name}: {sum:C}",
                Value = sum
            };
        }
        private static LineChartDataPoint[] GetPriceDistribution(IEnumerable<Product> products)
        {
            var step = 10.0m;
            var maxPrice = products.Max(p => p.Price);
            var currentMin = 0.0m;
            var currentMax = step;

            var data = new List<LineChartDataPoint>();
            while (currentMax < maxPrice)
            {
                var count = products.Count(p => p.Price >= currentMin && p.Price < currentMax);
                if (count > 0)
                {
                    data.Add(new LineChartDataPoint
                    {
                        Label = $"{currentMax:C}",
                        Value = count
                    });
                }
                currentMin += step;
                currentMax += step;
            }
            return data.ToArray();
        }
    }
}
