using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class BrandCrudService
    : AbstractCrudService<BrandEntity>
{
    private readonly VeloDbContext _db;

    public BrandCrudService(VeloDbContext db)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override DbSet<BrandEntity> Set => _db.Brands;

    public async override Task<bool> CanDeleteAsync(BrandEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Products.AnyAsync(p => p.BrandId == entity.Id);
    }

    public override IQueryable<BrandEntity> DefaultOrder(IQueryable<BrandEntity> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.DefaultOrder();
    }

    public override Expression<Func<BrandEntity, bool>> Search(string searchString)
    {
        ArgumentNullException.ThrowIfNull(searchString);

        if (int.TryParse(searchString, out int id))
        {
            return b => b.Id == id;
        }
        if (_db.IsPostgreSQL())
        {
            return b => EF.Functions.ILike(b.Name, $"%{searchString}%");
        }
        return b => EF.Functions.Like(b.Name, $"%{searchString}%");
    }
}
