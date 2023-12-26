using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.EntityFrameworkCore;
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
    private readonly IProductLabelService _productLabelService;
    private readonly IStatusPushService _statusPushService;
    private readonly IClock _clock;
    private readonly ITokenProvider _tokenProvider;

    public SellerService(ITransactionService transactionService, IProductLabelService productLabelService, IStatusPushService statusPushService, ITokenProvider tokenProvider, IClock clock, VeloDbContext db)
        : base(db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _productLabelService = productLabelService ?? throw new ArgumentNullException(nameof(productLabelService));
        _statusPushService = statusPushService ?? throw new ArgumentNullException(nameof(statusPushService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
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
            .ToArrayAsync();

        IReadOnlyList<ProductEntity> soldProducts = products.Where(p => p.StorageState == StorageState.Sold).ToArray();

        return new (seller)
        {
            CanPushStatus = _statusPushService.IsEnabled,
            Transactions = transactions,
            Procucts = products,
            AcceptedProductCount = products.Count,
            SettlementAmout = soldProducts.Sum(p => p.GetCommissionedPrice(basar)),
            NotSoldProductCount = products.Where(p => p.StorageState != StorageState.Sold).Count(),
            PickedUpProductCount = products.Where(p => p.StorageState == StorageState.Lost && p.ValueState == ValueState.Settled).Count(),
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

        string fileName = $"Seller-{sellerId}_ProductLabels.pdf";
        byte[] data = await _productLabelService.CreateLabelsAsync(products);
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

        IPaginatedList<CrudItemModel<SellerEntity>> items = await iq
            .AsPaginatedAsync(parameter.PageSize.Value, parameter.PageIndex, CreateItemModelAsync);
        return items;
    }

    public async Task<int> SettleAsync(int basarId, int sellerId)
    {
        IEnumerable<ProductEntity> sellersProducts = await _db.Products.GetForSellerAsync(basarId, sellerId);
        IEnumerable<int> productIdsToSettle = sellersProducts.Where(p => p.IsAllowed(TransactionType.Settlement)).Ids();

        int settlemenId = await _transactionService.SettleAsync(basarId, sellerId, productIdsToSettle);
        SellerEntity seller = await _db.Sellers.FirstByIdAsync(sellerId);
        seller.ValueState = ValueState.Settled;
        await _db.SaveChangesAsync();

        return settlemenId;
    }

    public async Task TriggerStatusPushAsync(int basarId, int sellerId)
    {
        await _statusPushService.PushSellerAsync(basarId, sellerId);
    }

    public override Task UpdateAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.TrimIBAN();
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
