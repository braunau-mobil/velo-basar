using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class CountryCrudService
    : AbstractCrudService<CountryEntity>
{
    private readonly VeloTexts _txt;
    private readonly VeloDbContext _db;

    public CountryCrudService(VeloTexts txt, VeloDbContext db)
        : base(db)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override DbSet<CountryEntity> Set => _db.Countries;

    public async override Task<bool> CanDeleteAsync(CountryEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Sellers.AnyAsync(s => s.CountryId == entity.Id);
    }

    public override IQueryable<CountryEntity> DefaultOrder(IQueryable<CountryEntity> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.DefaultOrder();
    }

    public override Expression<Func<CountryEntity, bool>> Search(string searchString)
    {
        ArgumentNullException.ThrowIfNull(searchString);

        if (int.TryParse(searchString, out int id))
        {
            return c => c.Id == id;
        }
        if (_db.IsPostgreSQL())
        {
            return c => EF.Functions.ILike(c.Name, $"%{searchString}%")
            || EF.Functions.ILike(c.Iso3166Alpha3Code, $"%{searchString}%");
        }
        return c => EF.Functions.Like(c.Name, $"%{searchString}%")
            || EF.Functions.Like(c.Iso3166Alpha3Code, $"%{searchString}%");
    }
}
