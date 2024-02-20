using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Parameter;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class BasarService
    : AbstractCrudService<BasarEntity, ListParameter>
    , IBasarService
{
    private readonly VeloDbContext _db;
    private readonly IBasarStatsService _statsService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IClock _clock;

    public BasarService(VeloDbContext db, IBasarStatsService statsService, IStringLocalizer<SharedResources> localizer, IClock clock)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _statsService = statsService ?? throw new ArgumentNullException(nameof(statsService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public override DbSet<BasarEntity> Set => _db.Basars;    

    public async override Task<bool> CanDeleteAsync(BasarEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !await _db.Transactions.AnyAsync(t => t.BasarId == entity.Id);
    }

    public override async Task<BasarEntity> CreateNewAsync()
    {
        BasarEntity basar = new()
        {
            Date = _clock.GetCurrentDateTime()
        };
        return await Task.FromResult(basar);
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

        Set.Update(entity);
        if (entity.State == ObjectState.Enabled)
        {
            await Set.Where(basar => basar != entity)
                .ForEachAsync(basar => basar.State = ObjectState.Disabled);
        }
        await _db.SaveChangesAsync();
    }

    public async Task<string> GetBasarNameAsync(int id)
        => await _db.Basars.AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => b.Name)
            .FirstAsync();

    public async Task<int?> GetActiveBasarIdAsync()
    {
        int id = await _db.Basars.AsNoTracking()
            .Where(b => b.State == ObjectState.Enabled)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();

        if (id == 0)
        {
            return null;
        }
        return id;
    }

    public async Task<BasarDetailsModel> GetDetailsAsync(int id)
    {
        BasarEntity basar = await _db.Basars.AsNoTracking()
            .FirstByIdAsync(id);

        BasarSettlementStatus settlementStatus = await _statsService.GetSettlementStatusAsync(id);

        IReadOnlyList<ProductEntity> acceptedProducts = await _statsService.GetAcceptedProductsAsync(id);
        IReadOnlyList<Tuple<TimeOnly, decimal>> soldProductTimestampsAndPrices = await _statsService.GetSoldProductTimestampsAndPricesAsync(id);

        return new BasarDetailsModel(basar, settlementStatus)
        {
            AcceptanceCount = await _statsService.GetAcceptanceCountAsync(id),
            AcceptedProductsAmount = _statsService.GetAcceptedProductsAmount(acceptedProducts),
            AcceptedProductsCount = _statsService.GetAcceptedProductsCount(acceptedProducts),
            AcceptedProductTypesByAmount = _statsService.GetAcceptedProductTypesWithAmount(acceptedProducts),
            AcceptedProductTypesByCount = _statsService.GetAcceptedProductTypesWithCount(acceptedProducts),
            LostProductsCount = _statsService.GetLostProductsCount(acceptedProducts),
            LockedProductsCount = _statsService.GetLockedProductsCount(acceptedProducts),
            PriceDistribution = _statsService.GetPriceDistribution(acceptedProducts),
            SaleCount = await _statsService.GetSaleCountAsync(id),
            SaleDistribution = _statsService.GetSaleDistribution(soldProductTimestampsAndPrices),
            SoldProductsAmount = _statsService.GetSoldProductsAmount(acceptedProducts),
            SoldProductsCount = _statsService.GetSoldProductsCount(acceptedProducts),
            SoldProductTypesByAmount = _statsService.GetSoldProductTypesWithAmount(acceptedProducts),
            SoldProductTypesByCount = _statsService.GetSoldProductTypesWithCount(acceptedProducts),
        };
    }

    protected override IQueryable<BasarEntity> OrderByDefault(IQueryable<BasarEntity> iq)
    {
        ArgumentNullException.ThrowIfNull(iq);

        return iq.OrderBy(basar => basar.Name);
    }

    protected override Expression<Func<BasarEntity, bool>> Search(string searchString)
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
