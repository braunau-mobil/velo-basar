using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Data
{
    public class PsqlNumberPool : INumberPool
    {
        private readonly DatabaseFacade _database;

        public PsqlNumberPool(DatabaseFacade database)
        {
            _database = database;
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int NextNumber(Basar basar, TransactionType transactionType)
        {
            Contract.Requires(basar != null);

            var type = (int)transactionType;
            using (var command = _database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"update \"Number\" set \"Value\"=\"Value\" + 1 where \"BasarId\" = {basar.Id} and \"Type\" = {type};select \"Value\" from \"Number\"  where \"BasarId\" = {basar.Id} and \"Type\" = {type}";
                _database.OpenConnection();
                var result = command.ExecuteScalar();
                _database.CloseConnection();
                return (int)result;
            }
        }
    }
}
