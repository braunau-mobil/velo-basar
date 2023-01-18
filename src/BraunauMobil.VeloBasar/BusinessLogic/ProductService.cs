﻿using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class ProductService
    : IProductService
{
    private readonly VeloDbContext _db;
    private readonly IProductLabelService _productLabelService;
    private readonly ITransactionService _transactionService;

    public ProductService(VeloDbContext db, IProductLabelService productLabelService, ITransactionService transactionService)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _productLabelService = productLabelService ?? throw new ArgumentNullException(nameof(productLabelService));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
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

    public async Task<ProductDetailsModel> GetDetailsAsync(int activeBasarId, int productId)
    {
        ProductEntity entity = await _db.Products
            .IncludeAll()
            .FirstByIdAsync(productId);

        IReadOnlyList<TransactionEntity> transactions = await _db.Transactions
            .WhereBasarAndProduct(activeBasarId, productId)
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
            .Include(product => product.Brand)
            .Include(product => product.Type)
            .Include(product => product.Session)
                .ThenInclude(session => session.Basar)
            .FirstByIdAsync(productId);

        string fileName = $"Product-{product.Id}_Label.pdf";
        byte[] data = await _productLabelService.CreateLabelAsync(product);
        return FileDataEntity.Pdf(fileName, data);
    }

    public async Task<IReadOnlyList<ProductEntity>> GetManyAsync(IList<int> ids)
    {
        IReadOnlyList<ProductEntity> items = await _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Type)
            .Include(product => product.Session)
            .Where(product => ids.Contains(product.Id))
            .ToArrayAsync();
        return items.OrderBy(product => ids.IndexOf(product.Id)).ToArray();
    }

    public async Task<IPaginatedList<ProductEntity>> GetManyAsync(int pageSize, int pageIndex, int activeBasarId, string searchString, StorageState? storageState, ValueState? valueState, int? brandId, int? productTypeId)
    {
        IQueryable<ProductEntity> iq = _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Type)
            .Include(product => product.Session)
            .Where(product => product.Session.BasarId == activeBasarId);

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

        if (brandId != null)
        {
            iq = iq.Where(product => product.BrandId == brandId.Value);
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
            return p => EF.Functions.ILike(p.Brand.Name, $"%{searchString}%")
            || (p.Color != null && EF.Functions.ILike(p.Color, $"%{searchString}%"))
            || EF.Functions.ILike(p.Description, $"%{searchString}%")
            || (p.FrameNumber != null && EF.Functions.ILike(p.FrameNumber, $"%{searchString}%"))
            || (p.TireSize != null && EF.Functions.ILike(p.TireSize, $"%{searchString}%"))
            || EF.Functions.ILike(p.Type.Name, $"%{searchString}%");
        }
        return p => EF.Functions.Like(p.Brand.Name, $"%{searchString}%")
            || (p.Color != null && EF.Functions.Like(p.Color, $"%{searchString}%"))
            || EF.Functions.Like(p.Description, $"%{searchString}%")
            || (p.FrameNumber != null && EF.Functions.Like(p.FrameNumber, $"%{searchString}%"))
            || (p.TireSize != null && EF.Functions.Like(p.TireSize, $"%{searchString}%"))
            || EF.Functions.Like(p.Type.Name, $"%{searchString}%");
    }
}