using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xan.Extensions;
using Npgsql;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Data;

public sealed class VeloDbContext
    : IdentityDbContext
{
    public VeloDbContext(IClock clock, DbContextOptions<VeloDbContext> options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(clock);
        this.AddTimestampHandler(clock);
    }

    public DbSet<AcceptSessionEntity> AcceptSessions => Set<AcceptSessionEntity>();

    public DbSet<BasarEntity> Basars => Set<BasarEntity>();

    public DbSet<CountryEntity> Countries => Set<CountryEntity>();

    public DbSet<FileDataEntity> Files => Set<FileDataEntity>();

    public DbSet<NumberEntity> Numbers => Set<NumberEntity>();

    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public DbSet<ProductToTransactionEntity> ProductToTransaction => Set<ProductToTransactionEntity>();

    public DbSet<ProductTypeEntity> ProductTypes => Set<ProductTypeEntity>();

    public DbSet<SellerEntity> Sellers => Set<SellerEntity>();

    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

    public DbSet<ZipCodeEntity> ZipCodes => Set<ZipCodeEntity>();

    public DbParameter CreateParameter(string name, object? value)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (IsPostgreSQL())
        {
            return new NpgsqlParameter(name, value);
        }
        return new SqlParameter(name, value);
    }

    public async Task CreateDatabaseAsync()
    {
        if (IsSQLITE())
        {
            await Database.EnsureCreatedAsync();
        }
        else
        {
            await Database.MigrateAsync();
        }
    }


    public async Task DropDatabaseAsync()
    {
        await Database.EnsureDeletedAsync();
        await SaveChangesAsync();
    }


    public async Task<bool> NeedsInitialSetupAsync()
        => !await Users.AnyAsync();

    public async Task<bool> NeedsMigrationAsync()
    {
        IEnumerable<string> pendingMigrations = await Database.GetPendingMigrationsAsync();
        return pendingMigrations.Any();
    }

    public async Task<bool> IsInitializedAsync()
        => await Users.AnyAsync();

    public bool IsPostgreSQL() => Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL";

    public bool IsSQLITE() => Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        builder.Entity<ProductToTransactionEntity>().HasKey(ap => new { ap.ProductId, ap.TransactionId });
        builder.Entity<NumberEntity>().HasKey(n => new { n.BasarId, n.Type });
        builder.Entity<ZipCodeEntity>().HasKey(z => new { z.Zip, z.CountryId });
        builder.Entity<TransactionEntity>()
            .HasOne(x => x.ParentTransaction)
            .WithMany(x => x.ChildTransactions)
            .HasForeignKey(x => x.ParentTransactionId)
            .IsRequired(false);
    }
}
