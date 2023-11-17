using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class NumberService
    : INumberService
{
    private readonly VeloDbContext _db;

    public NumberService(VeloDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<int> NextNumberAsync(int basarId, TransactionType transactionType)
    {
        using (IDbContextTransaction transaction = await _db.Database.BeginTransactionAsync())
        {
            await _db.Numbers
                .Where(number => number.BasarId == basarId && number.Type == transactionType)
                .ExecuteUpdateAsync(setters => setters.SetProperty(number => number.Value, number => number.Value + 1));

            NumberEntity number = await _db.Numbers
                .AsNoTracking()
                .FirstAsync(number => number.BasarId == basarId && number.Type == transactionType);

            await transaction.CommitAsync();

            return number.Value;
        }
    }
}
