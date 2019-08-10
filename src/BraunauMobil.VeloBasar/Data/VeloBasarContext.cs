﻿using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Npgsql;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloBasarContext : IdentityDbContext
    {
        private const string NextNumberSql = "update \"Number\" set \"Value\"=\"Value\" + 1 where \"BasarId\" = @BasarId and \"Type\" = @Type;select \"Value\" from \"Number\"  where \"BasarId\" = @BasarId and \"Type\" = @Type";

        public VeloBasarContext (DbContextOptions<VeloBasarContext> options)
            : base(options)
        {
        }

        public DbSet<Acceptance> Acceptance { get; set; }

        public DbSet<Basar> Basar { get; set; }

        public DbSet<Billing> Billing { get; set; }

        public DbSet<Cancellation> Cancellation { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<Number> Number { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Purchase> Purchase { get; set; }

        public DbSet<Seller> Seller { get; set; }

        public async Task<Basar> CreateNewBasarAsync(DateTime date, string name, decimal productCommission, decimal productDiscount, decimal sellerDiscount)
        {
            var basar = new Basar
            {
                Date = date,
                Name = name,
                ProductCommission = productCommission,
                ProductDiscount = productDiscount,
                SellerDiscount = sellerDiscount
            };
            await Basar.AddAsync(basar);
            await SaveChangesAsync();

            await CreateNewNumberAsync(basar.Id, TransactionType.Acceptance);
            await CreateNewNumberAsync(basar.Id, TransactionType.Billing);
            await CreateNewNumberAsync(basar.Id, TransactionType.Cancellation);
            await CreateNewNumberAsync(basar.Id, TransactionType.Purchase);

            return basar;
        }

        public async Task CreateNewNumberAsync(int basarId, TransactionType type)
        {
            var number = new Number
            {
                BasarId = basarId,
                Value = 0,
                Type = type
            };
            await Number.AddAsync(number);
            await SaveChangesAsync();
        }

        public int NextNumber(int basarId, TransactionType transactionType)
        {
            var number = -1;
            
            //  @todo i was ned wieso, aber wann i de drecks Connection in using pack, dann krachts da gewaltig
            var connection = Database.GetDbConnection() as NpgsqlConnection;
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = NextNumberSql;
                command.Parameters.AddWithValue("@BasarId", basarId);
                command.Parameters.AddWithValue("@Type", (int)transactionType);

                number  = (int)command.ExecuteScalar();
            }

            connection.Close();

            return number;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAcceptance>().HasKey(ap => new { ap.AcceptanceId, ap.ProductId });
            modelBuilder.Entity<BilledAcceptance>().HasKey(ba => new { ba.BillingId, ba.AcceptanceId });
            modelBuilder.Entity<PurchasedProduct>().HasKey(pp => new { pp.PurchaseId, pp.ProductId });
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
        }
    }
}
