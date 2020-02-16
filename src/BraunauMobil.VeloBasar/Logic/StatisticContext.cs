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
        private readonly IBasarContext _basarContext;
        private readonly IProductContext _productContext;
        private readonly ITransactionContext _transactionContext;
        private readonly IColorProvider _colorProvider;

        public StatisticContext(ITransactionContext transactionContext, IBasarContext basarContext, IProductContext productContext, IColorProvider colorProvider)
        {
            _transactionContext = transactionContext;
            _basarContext = basarContext;
            _productContext = productContext;
            _colorProvider = colorProvider;
        }

        public async Task<BasarStatistic> GetBasarStatisticAsnyc(int basarId)
        {
            var basar = await _basarContext.GetAsync(basarId);
            var products = await _productContext.GetProductsForBasar(basar).ToArrayAsync();
            var soldProducts = products.Where(p => p.StorageState == StorageState.Sold).ToArray();
            var sellerCount = await _transactionContext.GetMany(basar, TransactionType.Acceptance).Select(t => t.SellerId).Distinct().CountAsync();

            return new BasarStatistic
            {
                Basar = basar,
                SellerCount = sellerCount,
                AcceptedProductsCount = products.Length,
                AcceptedProductsByCount = GroupByProductType(products, FromCount),
                AcceptedProductsAmount = products.SumPrice(),
                AcceptedProductsByAmount = GroupByProductType(products, FromAmount),
                SoldProductsCount = soldProducts.Length,
                SoldProductsByCount = GroupByProductType(soldProducts, FromCount),
                SoldProductsAmount = soldProducts.SumPrice(),
                SoldProductsByAmount = GroupByProductType(soldProducts, FromAmount),
                GoneProductsCount = products.Count(p => p.IsGone()),
                LockedProductsCount = products.Count(p => p.IsLocked()),
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
                SettlementAmout = soldProducts.Sum(p => p.GetCommissionedPrice(basar)),
                NotSoldProductCount = products.Where(p => p.StorageState != StorageState.Sold).Count(),
                PickedUpProductCount = products.Where(p => p.StorageState == StorageState.Gone && p.ValueState == ValueState.Settled).Count(),
                SoldProductCount = soldProducts.Length
            };
        }

        private ChartDataPoint[] GroupByProductType(IEnumerable<Product> products, Func<IGrouping<ProductType, Product>, ChartDataPoint> getPoint)
        {
            var data = new List<ChartDataPoint>();
            foreach (var group in products.GroupBy(p => p.Type))
            {
                var point = getPoint(group);
                point.Color = _colorProvider[group.Key.Name];
                data.Add(point);
            }
            return data.ToArray();
        }
        private ChartDataPoint[] GetPriceDistribution(IEnumerable<Product> products)
        {
            if (!products.Any())
            {
                return Array.Empty<ChartDataPoint>();
            }

            var step = 10.0m;
            var maxPrice = products.Max(p => p.Price);
            var currentMin = 0.0m;
            var currentMax = step;

            var data = new List<ChartDataPoint>();
            while (currentMax < maxPrice)
            {
                var count = products.Count(p => p.Price >= currentMin && p.Price < currentMax);
                if (count > 0)
                {
                    data.Add(new ChartDataPoint
                    {
                        Label = $"{currentMax:C}",
                        Value = count,
                        Color = _colorProvider.Primary
                    });
                }
                currentMin += step;
                currentMax += step;
            }
            return data.ToArray();
        }

        private static ChartDataPoint FromCount(IGrouping<ProductType, Product> group)
        {
            var count = group.Count();
            return new ChartDataPoint
            {
                Label = $"{group.Key.Name}: {count}",
                Value = count
            };
        }
        private static ChartDataPoint FromAmount(IGrouping<ProductType, Product> group)
        {
            var sum = group.SumPrice();
            return new ChartDataPoint
            {
                Label = $"{group.Key.Name}: {sum:C}",
                Value = sum
            };
        }
    }
}
