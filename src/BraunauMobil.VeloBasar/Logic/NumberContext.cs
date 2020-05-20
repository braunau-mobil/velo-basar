using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class NumberContext : INumberContext
    {
        private readonly VeloRepository _db;

        public NumberContext(VeloRepository db)
        {
            _db = db;
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int NextNumber(Basar basar, TransactionType transactionType)
        {
            if (basar == null) throw new ArgumentNullException(nameof(basar));

            var type = (int)transactionType;
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"update \"Numbers\" set \"Value\"=\"Value\" + 1 where \"BasarId\" = {basar.Id} and \"Type\" = {type};select \"Value\" from \"Numbers\"  where \"BasarId\" = {basar.Id} and \"Type\" = {type}";
                _db.Database.OpenConnection();
                var result = command.ExecuteScalar();
                _db.Database.CloseConnection();
                if (result is int intResult)
                {
                    return intResult;
                }
                if (result is long longReusult)
                {
                    return (int)longReusult;
                }
                throw new NotSupportedException($"The type {result.GetType()} is not supported. ");
            }
        }
        public async Task CreateNewNumberAsync(Basar basar, TransactionType type)
        {
            var number = new Number
            {
                Basar = basar,
                Value = 0,
                Type = type
            };
            _db.Numbers.Add(number);
            await _db.SaveChangesAsync();
        }
    }
}
