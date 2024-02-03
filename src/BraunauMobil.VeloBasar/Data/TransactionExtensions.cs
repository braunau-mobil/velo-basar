using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Data;

public static class TransactionExtensions
{
    public static IQueryable<TransactionEntity> IncludeAll(this IQueryable<TransactionEntity> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);
        return transactions
            .Include(t => t.ParentTransaction)
            .Include(t => t.ChildTransactions)
                .ThenInclude(t => t.Products)
                    .ThenInclude(pt => pt.Product)
            .Include(t => t.Basar)
            .Include(t => t.Seller)
                .ThenInclude(s => s!.Country)
            .Include(t => t.Products)
                .ThenInclude(pt => pt.Product)
                    .ThenInclude(p => p.Session)
                        .ThenInclude(s => s.Seller)
            .Include(t => t.Products)
                .ThenInclude(pt => pt.Product)
                    .ThenInclude(p => p.Type)
            .AsSplitQuery()
            ;
    }

    public static async Task<DateTime> GetTimestampAsync(this IQueryable<TransactionEntity> transactions, int id)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        return await transactions.SelectByIdAsync(id, transaction => transaction.TimeStamp);
    }

    public static IQueryable<TransactionEntity> IncludeOnlyProducts(this IQueryable<TransactionEntity> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);
        return transactions
            .Include(t => t.Products)
                .ThenInclude(pt => pt.Product)
                    .ThenInclude(p => p.Session)
                        .ThenInclude(s => s.Seller)
            .Include(t => t.Products)
                .ThenInclude(pt => pt.Product)
                    .ThenInclude(p => p.Session)
                        .ThenInclude(s => s.Basar)
            .Include(t => t.Products)
                .ThenInclude(pt => pt.Product)
                    .ThenInclude(p => p.Type)
            ;
    }

    public static IQueryable<TransactionEntity> WhereBasarAndSeller(this IQueryable<TransactionEntity> transactions, int basarId, int sellerId)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        return transactions
            .Where(tx => tx.BasarId == basarId && tx.SellerId == sellerId);
    }

    public static IQueryable<TransactionEntity> WhereBasarAndType(this IQueryable<TransactionEntity> transactions, int basarId, TransactionType transactionType)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        return transactions
            .Where(tx => tx.BasarId == basarId && tx.Type == transactionType);
    }

    public static IQueryable<TransactionEntity> WhereBasarAndTypeAndSeller(this IQueryable<TransactionEntity> transactions, int basarId, TransactionType transactionType, int sellerId)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        return transactions
            .Where(tx => tx.BasarId == basarId && tx.Type == transactionType && tx.SellerId == sellerId);
    }

    public static IQueryable<TransactionEntity> WhereBasarAndTypeAndNumber(this IQueryable<TransactionEntity> transactions, int basarId, TransactionType type, int number)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        return transactions
            .Where(transaction => transaction.BasarId == basarId 
                && transaction.Type == type 
                && transaction.Number == number);
    }
}
