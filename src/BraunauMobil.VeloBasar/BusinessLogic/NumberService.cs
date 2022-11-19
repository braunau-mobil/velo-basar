using BraunauMobil.VeloBasar.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
        int type = (int)transactionType;
        using DbCommand command = _db.Database.GetDbConnection().CreateCommand();
        command.CommandText = @"update ""Numbers"" set ""Value""=""Value"" + 1 where ""BasarId"" = @basarId and ""Type"" = @type;select ""Value"" from ""Numbers""  where ""BasarId"" = @basarId and ""Type"" = @type";
        command.Parameters.Add(_db.CreateParameter("basarId", basarId));
        command.Parameters.Add(_db.CreateParameter("type", type));

        _db.Database.OpenConnection();
        object? result = await command.ExecuteScalarAsync();
        _db.Database.CloseConnection();
        
        if (result is int intResult)
        {
            return intResult;
        }
        
        if (result is long longReusult)
        {
            return (int)longReusult;
        }
        
        if (result == null)
        {
            throw new NotSupportedException($"SQL Statement returned null");
        }
        
        throw new NotSupportedException($"The type {result.GetType()} is not supported. ");
    }
}
