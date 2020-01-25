using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloRepository : IdentityDbContext
    {
        public VeloRepository (DbContextOptions<VeloRepository> options)
            : base(options)
        {
        }

        public DbSet<Basar> Basars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FileData> Files { get; set; }
        public DbSet<Number> Numbers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ProductsTransaction> Transactions { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ZipMap> ZipMap { get; set; }

        public bool IsInitialized()
        {
            return Database.GetService<IRelationalDatabaseCreator>().Exists();
        }
        public bool IsPostgreSQL() => Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Contract.Requires(modelBuilder != null);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductToTransaction>().HasKey(ap => new { ap.ProductId, ap.TransactionId });
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
            modelBuilder.Entity<ZipMap>().HasKey(z => new { z.Zip, z.CountryId });
        }
    }
}

