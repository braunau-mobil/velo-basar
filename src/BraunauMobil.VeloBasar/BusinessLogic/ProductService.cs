using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class ProductService
    : IProductService
{
    private readonly VeloDbContext _db;
    private readonly IDocumentService _documentService;
    private readonly ITransactionService _transactionService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ProductService(VeloDbContext db, IDocumentService documentService, ITransactionService transactionService, IStringLocalizer<SharedResources> localizer)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public async Task<bool> ExistsForBasarAsync(int basarId, int productId)
        => await _db.Products
            .Include(product => product.Session)
            .AnyAsync(product => product.Session.BasarId == basarId && product.Id == productId);

    public async Task<ProductEntity?> FindAsync(int id)
        => await _db.Products
            .IncludeAll()        
            .FirstOrDefaultByIdAsync(id);

    public async Task<ProductEntity> GetAsync(int id)
        => await _db.Products
            .IncludeAll()
            .FirstByIdAsync(id);

    public async Task<ProductDetailsModel> GetDetailsAsync(int id)
    {
        ProductEntity entity = await _db.Products
            .IncludeAll()
            .FirstByIdAsync(id);

        IReadOnlyList<TransactionEntity> transactions = await _db.Transactions
            .Where(tx => tx.Products.Any(pt => pt.ProductId == id))
            .ToArrayAsync();

        return new ProductDetailsModel(entity)
        {
            CanLock = entity.IsAllowed(TransactionType.Lock),
            CanSetAsLost = entity.IsAllowed(TransactionType.SetLost),
            CanUnlock = entity.IsAllowed(TransactionType.Unlock),
            Transactions = transactions
        };
    }

    public async Task<FileDataEntity> GetLabelAsync(int productId)
    {
        ProductEntity product = await _db.Products
            .Include(product => product.Type)
            .Include(product => product.Session)
                .ThenInclude(session => session.Basar)
            .FirstByIdAsync(productId);

        string fileName = _localizer[VeloTexts.LabelFileName, product.Id];
        byte[] data = await _documentService.CreateLabelAsync(product);
        return FileDataEntity.Pdf(fileName, data);
    }

    public async Task<IReadOnlyList<ProductEntity>> GetManyAsync(IList<int> ids)
    {
        IReadOnlyList<ProductEntity> items = await _db.Products
            .Include(product => product.Type)
            .Include(product => product.Session)
            .Where(product => ids.Contains(product.Id))
            .ToArrayAsync();
        return items.OrderBy(product => ids.IndexOf(product.Id)).ToArray();
    }

    public async Task<IPaginatedList<ProductEntity>> GetManyAsync(int pageSize, int pageIndex, int basarId, string searchString, StorageState? storageState, ValueState? valueState, string? brand, int? productTypeId)
    {
        IQueryable<ProductEntity> iq = _db.Products
            .Include(product => product.Type)
            .Include(product => product.Session)
            .Where(product => product.Session.BasarId == basarId);

        if (!string.IsNullOrEmpty(searchString))
        {
            iq = iq.Where(ProductSearch(searchString));
        }
        if (storageState != null)
        {
            iq = iq.Where(product => product.StorageState == storageState.Value);
        }

        if (valueState != null)
        {
            iq = iq.Where(product => product.ValueState == valueState.Value);
        }

        if (brand is not null)
        {
            iq = iq.Where(product => product.Brand == brand);
        }

        if (productTypeId != null)
        {
            iq = iq.Where(product => product.TypeId == productTypeId.Value);
        }

        return await iq
            .OrderById()
            .AsPaginatedAsync(pageSize, pageIndex);
    }

    public async Task LockAsync(int id, string notes)
    {
        ArgumentNullException.ThrowIfNull(notes);

        int basarId = await _db.Products.GetBasarIdAsync(id);
        await _transactionService.LockAsync(basarId, notes, id);
    }

    public async Task SetLostAsync(int id, string notes)
    {
        ArgumentNullException.ThrowIfNull(notes);

        int basarId = await _db.Products.GetBasarIdAsync(id);
        await _transactionService.SetLostAsync(basarId, notes, id);
    }

    public async Task UnlockAsync(int id, string notes)
    {
        ArgumentNullException.ThrowIfNull(notes);

        int basarId = await _db.Products.GetBasarIdAsync(id);
        await _transactionService.UnlockAsync(basarId, notes, id);
    }

    public async Task UpdateAsync(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    private Expression<Func<ProductEntity, bool>> ProductSearch(string searchString)
    {
        if (_db.IsPostgreSQL())
        {
            return p => EF.Functions.ILike(p.Brand, $"%{searchString}%")
            || (p.Color != null && EF.Functions.ILike(p.Color, $"%{searchString}%"))
            || EF.Functions.ILike(p.Description, $"%{searchString}%")
            || (p.FrameNumber != null && EF.Functions.ILike(p.FrameNumber, $"%{searchString}%"))
            || (p.TireSize != null && EF.Functions.ILike(p.TireSize, $"%{searchString}%"))
            || EF.Functions.ILike(p.Type.Name, $"%{searchString}%");
        }
        return p => EF.Functions.Like(p.Brand, $"%{searchString}%")
            || (p.Color != null && EF.Functions.Like(p.Color, $"%{searchString}%"))
            || EF.Functions.Like(p.Description, $"%{searchString}%")
            || (p.FrameNumber != null && EF.Functions.Like(p.FrameNumber, $"%{searchString}%"))
            || (p.TireSize != null && EF.Functions.Like(p.TireSize, $"%{searchString}%"))
            || EF.Functions.Like(p.Type.Name, $"%{searchString}%");
    }
}
