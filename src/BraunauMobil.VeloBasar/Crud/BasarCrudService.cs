using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class BasarCrudService
    : AbstractCrudService<BasarEntity>
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly VeloDbContext _db;

    public BasarCrudService(IStringLocalizer<SharedResources> localizer, VeloDbContext db)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public override DbSet<BasarEntity> Set => _db.Basars;

    public async override Task<bool> CanDeleteAsync(BasarEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Transactions.AnyAsync(t => t.BasarId == entity.Id);
    }

    public async override Task<int> CreateAsync(BasarEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        bool activate = entity.State == ObjectState.Enabled;
        entity.State = ObjectState.Disabled;

        _db.Basars.Add(entity);
        _db.Numbers.AddRange(Enum.GetValues<TransactionType>()
            .Select(transactionType =>
                new NumberEntity()
                {
                    Basar = entity,
                    Type = transactionType,
                    Value = 0,
                }
            ));
        await _db.SaveChangesAsync();

        if (activate)
        {
            await EnableAsync(entity.Id);
        }

        return entity.Id;
    }

    public override IQueryable<BasarEntity> DefaultOrder(IQueryable<BasarEntity> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.OrderBy(b => b.Date);
    }

    public async override Task DeleteAsync(int id)
    {
        if (await _db.Transactions.AnyAsync(t => t.BasarId == id))
        {
            throw new InvalidOperationException(_localizer[VeloTexts.CannotDeleteBasar, id]);
        }

        BasarEntity basar = await _db.Basars.FirstByIdAsync(id);
        _db.Numbers.RemoveRange(await _db.Numbers.GetForBasarAsync(id));
        _db.Basars.Remove(basar);
        await _db.SaveChangesAsync();
    }

    public async override Task EnableAsync(int id)
    {
        await _db.Basars.ForEachAsync(basar =>
        {
            if (basar.Id == id)
            {
                basar.State = ObjectState.Enabled;
            }
            else
            {
                basar.State = ObjectState.Disabled;
            }
        });
        await _db.SaveChangesAsync();
    }

    public async override Task DisableAsync(int id)
    {
        BasarEntity entity = await Set.FirstByIdAsync(id);
        entity.State = ObjectState.Disabled;
        await _db.AcceptSessions.ForEachAsync(session =>
        {
            _db.AcceptSessions.Remove(session);
        });
        await _db.SaveChangesAsync();
    }

    public async override Task UpdateAsync(BasarEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _db.Basars.Update(entity);
        if (entity.State == ObjectState.Enabled)
        {
            await _db.Basars.ForEachAsync(basar =>
            {
                if (basar.Id == entity.Id)
                {
                    basar.State = ObjectState.Enabled;
                }
                else
                {
                    basar.State = ObjectState.Disabled;
                }
            });
        }
        await _db.SaveChangesAsync();
    }

    public override Expression<Func<BasarEntity, bool>> Search(string searchString)
    {
        ArgumentNullException.ThrowIfNull(searchString);

        if (int.TryParse(searchString, out int id))
        {
            return b => b.Id == id;
        }
        if (_db.IsPostgreSQL())
        {
            return b => EF.Functions.ILike(b.Name, $"%{searchString}%")
                || (b.Location != null && EF.Functions.ILike(b.Location, $"%{searchString}%"));
        }
        return b => EF.Functions.Like(b.Name, $"%{searchString}%")
            || (b.Location != null && EF.Functions.Like(b.Location, $"%{searchString}%"));
    }
}
