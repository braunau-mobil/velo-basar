using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class ProductTypeCrudService
    : AbstractCrudService<ProductTypeEntity>
{
    private readonly VeloTexts _txt;
    private readonly VeloDbContext _db;

    public ProductTypeCrudService(VeloTexts txt, VeloDbContext db)
        : base(db)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override DbSet<ProductTypeEntity> Set => _db.ProductTypes;

    public async override Task<bool> CanDeleteAsync(ProductTypeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Products.AnyAsync(p => p.TypeId == entity.Id);
    }

    public override IQueryable<ProductTypeEntity> DefaultOrder(IQueryable<ProductTypeEntity> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.DefaultOrder();
    }

    public override Expression<Func<ProductTypeEntity, bool>> Search(string searchString)
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
