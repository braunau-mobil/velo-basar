using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class CountryService
    : AbstractCrudService<CountryEntity, ListParameter>
    , ICountryService
{
    private readonly VeloDbContext _db;

    public CountryService(VeloDbContext db)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override DbSet<CountryEntity> Set => _db.Countries;

    public async override Task<bool> CanDeleteAsync(CountryEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Sellers.AnyAsync(s => s.CountryId == entity.Id);
    }

    protected override IQueryable<CountryEntity> OrderByDefault(IQueryable<CountryEntity> iq)
    {
        ArgumentNullException.ThrowIfNull(iq);

        return _db.Countries.OrderBy(country => country.Name);
    }

    protected override Expression<Func<CountryEntity, bool>> Search(string searchString)
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
