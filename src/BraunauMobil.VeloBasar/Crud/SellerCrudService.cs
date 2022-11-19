using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Crud;

public class SellerCrudService
    : AbstractCrudService<SellerEntity>
{
    private readonly VeloDbContext _db;
    private readonly ITokenProvider _tokenProvider;
    private readonly IClock _clock;

    public SellerCrudService(ITokenProvider tokenProvider, IClock clock, VeloDbContext db)
        : base(db)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override DbSet<SellerEntity> Set => _db.Sellers;

    public async override Task<bool> CanDeleteAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Products.AnyAsync(p => p.TypeId == entity.Id);
    }

    public async override Task<int> CreateAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.TrimIBAN();
        entity.UpdateNewsletterPermissions(_clock);
        int id = await base.CreateAsync(entity);

        entity.Token = _tokenProvider.CreateToken(entity);
        await _db.SaveChangesAsync();

        return id;
    }

    public override IQueryable<SellerEntity> DefaultOrder(IQueryable<SellerEntity> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.DefaultOrder();
    }

    public override Task UpdateAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.TrimIBAN();
        entity.UpdateNewsletterPermissions(_clock);
        return base.UpdateAsync(entity);
    }

    public override Expression<Func<SellerEntity, bool>> Search(string searchString)
    {
        ArgumentNullException.ThrowIfNull(searchString);

        if (int.TryParse(searchString, out int id))
        {
            return b => b.Id == id;
        }
        if (_db.IsPostgreSQL())
        {
            return s => EF.Functions.ILike(s.FirstName, $"%{searchString}%")
              || EF.Functions.ILike(s.LastName, $"%{searchString}%")
              || EF.Functions.ILike(s.Street, $"%{searchString}%")
              || EF.Functions.ILike(s.City, $"%{searchString}%")
              || EF.Functions.ILike(s.Country.Name, $"%{searchString}%")
              || s.BankAccountHolder != null && EF.Functions.ILike(s.BankAccountHolder, $"%{searchString}%");
        }
        return s => EF.Functions.Like(s.FirstName, $"%{searchString}%")
          || EF.Functions.Like(s.LastName, $"%{searchString}%")
          || EF.Functions.Like(s.Street, $"%{searchString}%")
          || EF.Functions.Like(s.City, $"%{searchString}%")
          || EF.Functions.Like(s.Country.Name, $"%{searchString}%")
          || s.BankAccountHolder != null && EF.Functions.Like(s.BankAccountHolder, $"%{searchString}%");
    }
}
