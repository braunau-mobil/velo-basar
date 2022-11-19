﻿using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class TransactionService
    : ITransactionService
{
    private readonly INumberService _numberService;
    private readonly ITransactionDocumentService _transactionDocumentService;
    private readonly IStatusPushService _statusPushService;
    private readonly VeloDbContext _db;
    private readonly IProductLabelService _productLabelService;
    private readonly IClock _clock;
    private readonly VeloTexts _txt;

    public TransactionService(INumberService numberService, ITransactionDocumentService transactionDocumentService, IStatusPushService statusPushService, VeloDbContext db, IProductLabelService productLabelService, IClock clock, VeloTexts txt)
    {
        _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
        _transactionDocumentService = transactionDocumentService ?? throw new ArgumentNullException(nameof(transactionDocumentService));
        _statusPushService = statusPushService ?? throw new ArgumentNullException(nameof(statusPushService));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _productLabelService = productLabelService ?? throw new ArgumentNullException(nameof(productLabelService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
    }

    public async Task<int> AcceptAsync(int basarId, int sellerId, IEnumerable<int> productIds)
    {
        ArgumentNullException.ThrowIfNull(productIds);

        return await CreateAsync(TransactionType.Acceptance, basarId, productIds, sellerId: sellerId);
    }

    public async Task<int> CancelAsync(int basarId, int saleId, IEnumerable<int> productIds)
    {
        ArgumentNullException.ThrowIfNull(productIds);

        TransactionEntity sale = await _db.Transactions
            .IncludeOnlyProducts()
            .FirstByIdAsync(saleId);

        foreach (int procutId in productIds)
        {
            ProductToTransactionEntity product = sale.Products.First(p => p.ProductId == procutId);
            sale.Products.Remove(product);
        }

        FileDataEntity? fileData = null;
        if (sale.DocumentId.HasValue)
        {
            fileData = await _db.Files.FirstByIdAsync(sale.DocumentId.Value);
            await UpdateFileDataAsync(fileData, sale);
        }
        else
        {
            fileData = new FileDataEntity
            {
                ContentType = FileDataEntity.PdfContentType
            };
            _db.Files.Add(fileData);
            await UpdateFileDataAsync(fileData, sale);

            sale.DocumentId = fileData.Id;
            await _db.SaveChangesAsync();
        }

        return await CreateAsync(TransactionType.Cancellation, basarId, productIds, parent: sale);
    }

    public async Task<int> CheckoutAsync(int basarId, IEnumerable<int> productIds)
    {
        ArgumentNullException.ThrowIfNull(productIds);

        return await CreateAsync(TransactionType.Sale, basarId, productIds);
    }

    public async Task<TransactionEntity?> FindAsync(int basarId, TransactionType type, int number)
        => await _db.Transactions
            .IncludeAll()
            .Where(basarId, type, number)
            .FirstOrDefaultAsync();

    public async Task<FileDataEntity> GetAcceptanceLabelsAsync(int id)
    {
        DateTime timeStamp = await _db.Transactions.GetTimestampAsync(id);

        IReadOnlyList<ProductEntity> products = await _db.Transactions
            .IncludeOnlyProducts()
            .Where(transaction => transaction.Id == id)
            .SelectMany(transaction => transaction.Products.Select(pt => pt.Product))
            .ToArrayAsync();

        string fileName = GetTransactionFileName(timeStamp, TransactionType.Acceptance, id, $"_{_txt.Labels}");
        byte[] data = await _productLabelService.CreateLabelsAsync(products);
        return FileDataEntity.Pdf(fileName, data);
    }

    public async Task<TransactionEntity> GetAsync(int id)
        => await GetAsync(id, 0m);

    public async Task<TransactionEntity> GetAsync(int id, decimal amountGiven)
    {
        TransactionEntity transaction = await _db.Transactions
            .IncludeAll()
            .FirstByIdAsync(id);
        transaction.Change = ChangeInfo.CreateFor(transaction, amountGiven);
        return transaction;
    }

    public async Task<FileDataEntity> GetDocumentAsync(int id)
    {
        TransactionEntity transaction = await _db.Transactions
            .IncludeAll()
            .FirstByIdAsync(id);

        FileDataEntity? fileData = null;
        if (transaction.DocumentId.HasValue)
        {
            fileData = await _db.Files.FirstByIdAsync(transaction.DocumentId.Value);
            if (transaction.HasDocument)
            {
                return fileData;
            }
        }

        if (fileData == null)
        {
            fileData = new FileDataEntity
            {
                ContentType = FileDataEntity.PdfContentType
            };
            _db.Files.Add(fileData);
            await UpdateFileDataAsync(fileData, transaction);

            transaction.DocumentId = fileData.Id;
            await _db.SaveChangesAsync();
        }
        else
        {
            await UpdateFileDataAsync(fileData, transaction);
        }

        return fileData;
    }

    public async Task<TransactionEntity> GetLatestAsync(int basarId, int productId)
        => await _db.Transactions
            .IncludeAll()
            .Where(transaction => transaction.Products.Any(pt => pt.ProductId == productId))
            .OrderByDescending(transaction => transaction.TimeStamp)
            .FirstAsync();

    public async Task<IPaginatedList<TransactionEntity>> GetManyAsync(int pageSize, int pageIndex, int basarId, TransactionType? type = null, string? searchString = null)
    {
        IQueryable<TransactionEntity> iq = _db.Transactions
            .IncludeAll()
            .Where(tx => tx.BasarId == basarId);
        if (type.HasValue)
        {
            iq = iq.Where(entity => entity.Type == type.Value);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            iq = iq.Where(Search(searchString));
        }
        iq = iq.OrderBy(tx => tx.TimeStamp);

        return await iq.AsPaginatedAsync(pageSize, pageIndex);
    }

    public async Task<IReadOnlyList<ProductEntity>> GetProductsToCancelAsync(int id)
    {
        IReadOnlyList<ProductEntity> products = await _db.Transactions
            .IncludeOnlyProducts()
            .Where(transaction => transaction.Id == id)
            .SelectMany(transaction => transaction.Products.Select(product => product.Product))
            .ToArrayAsync();

        return products
            .Where(product => product.IsAllowed(TransactionType.Cancellation))
            .ToArray();
    }

    public async Task<int> LockAsync(int basarId, string? notes, int productId)
        => await CreateAsync(TransactionType.Lock, basarId, new[] { productId }, notes);

    public async Task<int> SetLostAsync(int basarId, string? notes, int productId)
        => await CreateAsync(TransactionType.SetLost, basarId, new[] { productId }, notes);

    public async Task<int> SettleAsync(int basarId, int sellerId, IEnumerable<int> productIds)
    {
        ArgumentNullException.ThrowIfNull(productIds);

        TransactionEntity cancellation = await CreateNewAsync(TransactionType.Settlement, basarId, productIds, sellerId: sellerId);
        int id = await CreateAsync(cancellation);
        return id;
    }

    public async Task<int> UnlockAsync(int basarId, string? notes, int productId)
        => await CreateAsync(TransactionType.Unlock, basarId, new[] { productId }, notes);

    private async Task<int> CreateAsync(TransactionType type, int basarId, IEnumerable<int> productIds, string? notes = null, int? sellerId = null, TransactionEntity? parent = null)
    {
        TransactionEntity transaction = await CreateNewAsync(type, basarId, productIds, notes, sellerId, parent);
        int id = await CreateAsync(transaction);
        return id;
    }

    private async Task<TransactionEntity> CreateNewAsync(TransactionType type, int basarId, IEnumerable<int> productIds, string? notes = null, int? sellerId = null, TransactionEntity? parent = null)
    {
        IReadOnlyList<ProductEntity> products = await _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Type)
            .Include(product => product.Session)
                .ThenInclude(session => session.Seller)
            .GetManyAsync(productIds);
        if (productIds.Any() && !products.Any(product => product.IsAllowed(type)))
        {
            throw new InvalidOperationException($"Transaction {type} is not allowed.");
        }

        BasarEntity basar = await _db.Basars.FirstByIdAsync(basarId);

        TransactionEntity transaction = new()
        {
            Basar = basar,
            BasarId = basarId,
            TimeStamp = _clock.GetCurrentDateTime(),
            Type = type,
        };
        if (sellerId.HasValue)
        {
            SellerEntity seller = await _db.Sellers.FirstByIdAsync(sellerId.Value);
            transaction.Seller = seller;
            transaction.SellerId = seller.Id;
        }
        if (notes != null)
        {
            transaction.Notes = notes;
        }
        if (parent != null)
        {
            transaction.ParentTransaction = parent;
            transaction.ParentTransactionId = parent.Id;
        }
        foreach (ProductEntity productEntity in products)
        {
            ProductToTransactionEntity productToTransactionEntity = new(transaction, productEntity);
            transaction.Products.Add(productToTransactionEntity);
        }
        return transaction;
    }

    private async Task<int> CreateAsync(TransactionEntity transaction)
    {
        transaction.Number = await _numberService.NextNumberAsync(transaction.BasarId, transaction.Type);
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        if (transaction.NeedsStatusPush)
        {
            await _statusPushService.PushAwayAsync(transaction);
        }

        return transaction.Id;
    }

    private Expression<Func<TransactionEntity, bool>> Search(string searchString)
    {
        if (_db.IsPostgreSQL())
        {
            return x => x.Seller == null 
              || EF.Functions.ILike(x.Seller.FirstName, $"%{searchString}%")
              || EF.Functions.ILike(x.Seller.LastName, $"%{searchString}%")
              || EF.Functions.ILike(x.Seller.City, $"%{searchString}%")
              || EF.Functions.ILike(x.Seller.Country.Name, $"%{searchString}%")
              || x.Seller.BankAccountHolder != null && EF.Functions.ILike(x.Seller.BankAccountHolder, $"%{searchString}%");
        }
        return x => x.Seller == null
          || EF.Functions.Like(x.Seller.FirstName, $"%{searchString}%")
          || EF.Functions.Like(x.Seller.LastName, $"%{searchString}%")
          || EF.Functions.Like(x.Seller.City, $"%{searchString}%")
          || EF.Functions.Like(x.Seller.Country.Name, $"%{searchString}%")
          || x.Seller.BankAccountHolder != null && EF.Functions.Like(x.Seller.BankAccountHolder, $"%{searchString}%");
    }

    private string GetTransactionFileName(DateTime timeStamp, TransactionType type, int id, string suffix = "")
        => $"{timeStamp:s}_{_txt.Singular(type)}-{id}{suffix}.pdf";

    private async Task UpdateFileDataAsync(FileDataEntity toUpdate, TransactionEntity transaction)
    {
        toUpdate.FileName = GetTransactionFileName(transaction.TimeStamp, transaction.Type, transaction.Id);
        toUpdate.Data = await _transactionDocumentService.CreateAsync(transaction);
        await _db.SaveChangesAsync();
    }
}
