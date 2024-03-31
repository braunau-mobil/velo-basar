using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SellerService
    : AbstractCrudService<SellerEntity, SellerListParameter>
    , ISellerService
{
    private readonly VeloDbContext _db;
    private readonly ITransactionService _transactionService;
    private readonly IDocumentService _documentService;
    private readonly IStatusPushService _statusPushService;
    private readonly IClock _clock;
    private readonly ITokenProvider _tokenProvider;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public SellerService(ITransactionService transactionService, IDocumentService documentService, IStatusPushService statusPushService, ITokenProvider tokenProvider, IClock clock, VeloDbContext db, IStringLocalizer<SharedResources> localizer)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _statusPushService = statusPushService ?? throw new ArgumentNullException(nameof(statusPushService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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

        entity.UnifyIBAN();
        entity.UpdateNewsletterPermissions(_clock);
        int id = await base.CreateAsync(entity);

        entity.Token = _tokenProvider.CreateToken(entity);
        await _db.SaveChangesAsync();

        return id;
    }

    public async Task<bool> ExistsAsync(int id)
        => await _db.Sellers.AnyAsync(seller => seller.Id == id);

    public async Task<SellerDetailsModel> GetDetailsAsync(int basarId, int sellerId)
    {
        BasarEntity basar = await _db.Basars.FirstByIdAsync(basarId);
        SellerEntity seller = await _db.Sellers
            .Include(seller => seller.Country)
            .FirstByIdAsync(sellerId);

        IReadOnlyList<TransactionEntity> transactions = await _db.Transactions
            .IncludeOnlyProducts()
            .WhereBasarAndSeller(basarId, seller.Id)
            .ToArrayAsync();
        IReadOnlyList<ProductEntity> products = await _db.Products
            .WhereBasarAndSeller(basarId, sellerId)
            .Include(product => product.Type) 
            .Include(product => product.Session)
            .OrderBy(product => product.Id)
            .ToArrayAsync();

        IReadOnlyList<ProductEntity> soldProducts = products.Where(p => p.StorageState == StorageState.Sold).ToArray();

        return new (seller)
        {
            BasarId = basarId,
            CanPushStatus = _statusPushService.IsEnabled,
            Transactions = transactions,
            Products = products,
            AcceptedProductCount = products.Count,
            SettlementAmout = soldProducts.Sum(p => p.GetCommissionedPrice(basar)),
            NotSoldProductCount = products.Count(p => p.StorageState != StorageState.Sold),
            PickedUpProductCount = products.Count(p => p.WasPickedUp),
            SoldProductCount = soldProducts.Count
        };
    }

    public async Task<FileDataEntity> GetLabelsAsync(int basarId, int sellerId)
    {
        IEnumerable<ProductEntity> products = await _db.Products
            .Include(product => product.Type)
            .Include(product => product.Session)
                .ThenInclude(session => session.Basar)
            .Where(product => product.Session.BasarId == basarId && product.Session.SellerId == sellerId)
            .ToArrayAsync();

        string fileName = _localizer[VeloTexts.SellerLabelsFileName, sellerId];
        byte[] data = await _documentService.CreateLabelsAsync(products);
        return FileDataEntity.Pdf(fileName, data);
    }

    public async Task<IReadOnlyList<SellerEntity>> GetManyAsync(string firstName, string lastName)
    {
        return await _db.Sellers
            .Include(seller => seller.Country)
            .Where(SellerSearch(firstName, lastName))
            .DefaultOrder()
            .ToArrayAsync();
    }

    public override async Task<IPaginatedList<CrudItemModel<SellerEntity>>> GetManyAsync(SellerListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        IQueryable<SellerEntity> iq = _db.Sellers.Include(seller => seller.Country);
        if (!string.IsNullOrEmpty(parameter.SearchString))
        {
            iq = iq.Where(Search(parameter.SearchString));
        }
        if (parameter.State.HasValue)
        {
            iq = iq.Where(basar => basar.State == parameter.State.Value);
        }
        if (parameter.ValueState.HasValue)
        {
            iq = iq.Where(seller => seller.ValueState == parameter.ValueState.Value);
        }
        iq = OrderByDefault(iq);

        if (parameter.SettlementType.HasValue)
        {
            IReadOnlyList<SellerEntity> sellers = await iq.ToArrayAsync();
            List<CrudItemModel<SellerEntity>> items = [];
            foreach (SellerEntity seller in sellers)
            {
                CrudItemModel<SellerEntity> item = await CreateItemModelAsync(parameter.BasarId, seller);
                if (item.Entity.SettlementType == parameter.SettlementType.Value)
                {
                    items.Add(item);
                }
            }
            return await items.AsPaginatedAsync(parameter.PageSize.Value, parameter.PageIndex);
        }
        else
        {
            IPaginatedList<CrudItemModel<SellerEntity>> items = await iq
                .AsPaginatedAsync(parameter.PageSize.Value, parameter.PageIndex, async seller => await CreateItemModelAsync(parameter.BasarId, seller));
            return items;
        }
    }

    public async Task<int> SettleAsync(int basarId, int sellerId)
    {
        IEnumerable<ProductEntity> sellersProducts = await _db.Products.GetForBasarAndSellerAsync(basarId, sellerId);
        IEnumerable<int> productIdsToSettle = sellersProducts.Where(p => p.IsAllowed(TransactionType.Settlement)).Ids();

        return await _transactionService.SettleAsync(basarId, sellerId, productIdsToSettle);
    }

    public async Task TriggerStatusPushAsync(int basarId, int sellerId)
    {
        await _statusPushService.PushSellerAsync(basarId, sellerId);
    }

    public override Task UpdateAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.UnifyIBAN();
        entity.UpdateNewsletterPermissions(_clock);
        return base.UpdateAsync(entity);
    }

    protected override IQueryable<SellerEntity> OrderByDefault(IQueryable<SellerEntity> iq)
    {
        ArgumentNullException.ThrowIfNull(iq);

        return iq.DefaultOrder();
    }

    protected override Expression<Func<SellerEntity, bool>> Search(string searchString)
    {
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

    private async Task<CrudItemModel<SellerEntity>> CreateItemModelAsync(int basarId, SellerEntity entity)
    {
        //  Default = Must come by
        entity.SettlementType = SellerSettlementType.OnSite;

        if (entity.IBAN is not null)
        {
            //  Products to pickup are available/locked (not dontatable)
            int productsToPickupCount = await _db.Products.AsNoTracking()
                .Include(product => product.Session)
                    .ThenInclude(session => session.Seller)
                .Where(product => product.Session.BasarId == basarId && product.Session.SellerId == entity.Id
                    && product.ValueState == ValueState.NotSettled && product.StorageState != StorageState.NotAccepted && !product.DonateIfNotSold && (product.StorageState == StorageState.Available || product.StorageState == StorageState.Locked))
                .CountAsync();
            if (productsToPickupCount == 0)
            {
                entity.SettlementType = SellerSettlementType.Remote;
            }
        }
        else
        {
            //  Products to pickup are sold/lost (money) or available/locked (not dontatable)
            int productsToPickupCount = await _db.Products.AsNoTracking()
                .Include(product => product.Session)
                    .ThenInclude(session => session.Seller)
                .Where(product => product.Session.BasarId == basarId && product.Session.SellerId == entity.Id
                    && product.ValueState == ValueState.NotSettled && product.StorageState != StorageState.NotAccepted && (product.StorageState == StorageState.Sold || product.StorageState == StorageState.Lost || (product.StorageState == StorageState.Available && !product.DonateIfNotSold) || (product.StorageState == StorageState.Locked && !product.DonateIfNotSold)))
                .CountAsync();
            if (productsToPickupCount == 0)
            {
                entity.SettlementType = SellerSettlementType.Remote;
            }
        }

        return await CreateItemModelAsync(entity);
    }

    private Expression<Func<SellerEntity, bool>> SellerSearch(string firstName, string lastName)
    {
        if (_db.IsPostgreSQL())
        {
            if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return s => EF.Functions.ILike(s.FirstName, $"{firstName}%");
            }
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return s => EF.Functions.ILike(s.LastName, $"{lastName}%");
            }

            return s => EF.Functions.ILike(s.FirstName, $"{firstName}%")
              && EF.Functions.ILike(s.LastName, $"{lastName}%");
        }
        if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
        {
            return s => EF.Functions.Like(s.FirstName, $"{firstName}%");
        }
        else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            return s => EF.Functions.Like(s.LastName, $"{lastName}%");
        }

        return s => EF.Functions.Like(s.FirstName, $"{firstName}%")
          && EF.Functions.Like(s.LastName, $"{lastName}%");
    }
}
