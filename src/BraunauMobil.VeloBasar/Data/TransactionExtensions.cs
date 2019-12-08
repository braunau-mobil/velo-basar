using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Data
{
    public static class TransactionExtensions
    {
        public static IQueryable<ProductsTransaction> GetMany(this IQueryable<ProductsTransaction> transactions, TransactionType transactionType, Basar basar, int sellerId)
        {
            return transactions.GetMany(transactionType, basar, t => t.SellerId == sellerId);
        }
        public static IQueryable<ProductsTransaction> GetMany(this IQueryable<ProductsTransaction> transactions, TransactionType transactionType, Basar basar, Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            var result = transactions.Where(t => t.Type == transactionType && t.Basar.Id == basar.Id);

            if (additionalPredicate != null)
            {
                return result.Where(additionalPredicate).IncludeAll();
            }

            return result.IncludeAll();
        }
        public static IQueryable<ProductsTransaction> IncludeAll(this IQueryable<ProductsTransaction> transactions)

        {
            return transactions
                .Include(t => t.Basar)
                .Include(t => t.Seller)
                    .ThenInclude(s => s.Country)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Brand)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Type);
        }
    }
}
