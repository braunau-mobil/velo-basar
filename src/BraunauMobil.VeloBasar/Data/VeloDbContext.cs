using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using Xan.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace BraunauMobil.VeloBasar.Data;

public sealed class VeloDbContext
    : IdentityDbContext
{
    private readonly IClock _clock;

    public VeloDbContext(IClock clock, DbContextOptions<VeloDbContext> options)
        : base(options)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public DbSet<AcceptSessionEntity> AcceptSessions => Set<AcceptSessionEntity>();

    public DbSet<BasarEntity> Basars => Set<BasarEntity>();

    public DbSet<BrandEntity> Brands => Set<BrandEntity>();

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

    public bool IsInitialized()
    {
        return Database.GetService<IRelationalDatabaseCreator>().Exists();
    }

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (IHasTimestamps entity in ChangeTracker.Entries().AddedEntities<IHasTimestamps>())
        {
            entity.CreatedAt = _clock.GetCurrentDateTime();
            entity.UpdatedAt = _clock.GetCurrentDateTime();
        }      

        foreach (IHasTimestamps entity in ChangeTracker.Entries().ModifedEntities<IHasTimestamps>())
        {
            entity.UpdatedAt = _clock.GetCurrentDateTime();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
