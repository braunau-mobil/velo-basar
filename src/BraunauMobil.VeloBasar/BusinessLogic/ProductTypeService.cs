using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class ProductTypeService
    : AbstractCrudService<ProductTypeEntity, ListParameter>
    , IProductTypeService
{
    private readonly VeloDbContext _db;

    public ProductTypeService(VeloDbContext db)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override DbSet<ProductTypeEntity> Set => _db.ProductTypes;

    public async override Task<bool> CanDeleteAsync(ProductTypeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Products.AnyAsync(p => p.TypeId == entity.Id);
    }

    protected override IQueryable<ProductTypeEntity> OrderByDefault(IQueryable<ProductTypeEntity> iq)
    {
        ArgumentNullException.ThrowIfNull(iq);

        return _db.ProductTypes.OrderBy(c => c.Name);
    }

    protected override Expression<Func<ProductTypeEntity, bool>> Search(string searchString)
    {
        ArgumentNullException.ThrowIfNull(searchString);

        if (int.TryParse(searchString, out int id))
        {
            return c => c.Id == id;
        }
        if (_db.IsPostgreSQL())
        {
            return c => EF.Functions.ILike(c.Name, $"%{searchString}%");
        }
        return c => EF.Functions.Like(c.Name, $"%{searchString}%");
    }
}
