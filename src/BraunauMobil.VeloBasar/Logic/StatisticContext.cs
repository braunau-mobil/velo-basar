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
        private readonly ISellerContext _sellerContext;

        public StatisticContext(IStringLocalizer<SharedResource> localizer, ITransactionContext transactionContext, IBasarContext basarContext, IProductContext productContext, ISellerContext sellerContext)
        {
            _localizer = localizer;
            _transactionContext = transactionContext;
            _basarContext = basarContext;
            _productContext = productContext;
            _sellerContext = sellerContext;
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
                SettlementAmout = soldProducts.Sum(p => p.Price),
                NotSoldProductCount = products.Where(p => p.StorageState != StorageState.Sold).Count(),
                PickedUpProductCount = products.Where(p => p.StorageState == StorageState.Gone && p.ValueState == ValueState.Settled).Count(),
                SoldProductCount = soldProducts.Length
            };
        }
        public async Task<TransactionStatistic[]> GetTransactionStatistics(Basar basar, TransactionType type, int sellerId)
        {
            return await _transactionContext.GetMany(basar, type, sellerId).AsNoTracking().Select(a => new TransactionStatistic
            {
                Transaction = a,
                ProductCount = a.Products.Count,
                Amount = a.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }

        private ChartDataPoint[] GroupByProductType(IEnumerable<Product> products, Func<IGrouping<ProductType, Product>, ChartDataPoint> getPoint)
        {
            var data = new List<ChartDataPoint>();
            var colors = new ColorDispenser();
            foreach (var group in products.GroupBy(p => p.Type))
            {
                var point = getPoint(group);
                point.Color = colors.Next();
                data.Add(point);
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
        private static ChartDataPoint[] GetPriceDistribution(IEnumerable<Product> products)
        {
            if (!products.Any())
            {
                return Array.Empty<ChartDataPoint>();
            }

            var step = 10.0m;
            var maxPrice = products.Max(p => p.Price);
            var currentMin = 0.0m;
            var currentMax = step;
            var colors = new ColorDispenser();
            var color = colors.Next();

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
                        Color = color
                    });
                }
                currentMin += step;
                currentMax += step;
            }
            return data.ToArray();
        }
    }
}
