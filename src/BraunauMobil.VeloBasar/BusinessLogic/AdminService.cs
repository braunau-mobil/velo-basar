﻿using BraunauMobil.VeloBasar.Pdf;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public class AdminService
    : IAdminService
{
    private readonly IProductLabelService _productLabelService;
    private readonly ITransactionDocumentService _transactionDocumentService;
    private readonly IDataGeneratorService _dataGeneratorService;

    public AdminService(IProductLabelService productLabelService, ITransactionDocumentService transactionDocumentService, IDataGeneratorService dataGeneratorService)
    {
        _productLabelService = productLabelService ?? throw new ArgumentNullException(nameof(productLabelService));
        _transactionDocumentService = transactionDocumentService ?? throw new ArgumentNullException(nameof(transactionDocumentService));
        _dataGeneratorService = dataGeneratorService ?? throw new ArgumentNullException(nameof(dataGeneratorService));
        _dataGeneratorService.Contextualize(new DataGeneratorConfiguration());
    }

    public async Task<FileDataEntity> CreateSampleAcceptanceDocumentAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = _dataGeneratorService.NextSeller(country);
        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled)
        };

        TransactionEntity transaction = NextTransaction(TransactionType.Acceptance, basar, seller, products);
        return await CreateTransactionDocumentAsync(transaction);
    }

    public async Task<FileDataEntity> CreateSampleLabelsAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = _dataGeneratorService.NextSeller(country);
        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled)
        };
        byte[] data = await _productLabelService.CreateLabelsAsync(products);
        return FileDataEntity.Pdf("Sample_Labels.pdf", data);
    }

    public async Task<FileDataEntity> CreateSampleSaleDocumentAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = _dataGeneratorService.NextSeller(country);
        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Sold, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Sold, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Sold, ValueState.NotSettled)
        };

        TransactionEntity transaction = NextTransaction(TransactionType.Sale, basar, null, products);
        return await CreateTransactionDocumentAsync(transaction);
    }

    public async Task<FileDataEntity> CreateSampleSettlementDocumentAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = _dataGeneratorService.NextSeller(country);
        seller.IBAN = "AT536616326924127723";

        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Sold, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Sold, ValueState.NotSettled)
        };

        TransactionEntity transaction = NextTransaction(TransactionType.Settlement, basar, seller, products);
        return await CreateTransactionDocumentAsync(transaction);
    }

    private async Task<FileDataEntity> CreateTransactionDocumentAsync(TransactionEntity transaction)
    {
        byte[] data = await _transactionDocumentService.CreateAsync(transaction);
        return FileDataEntity.Pdf($"Sample_{transaction.Type}.pdf", data);
    }

    private ProductEntity NextProduct(BasarEntity basar, SellerEntity seller, StorageState? storageState = null, ValueState? valueState = null)
    {
        AcceptSessionEntity acceptSession = new()
        {
            BasarId = basar.Id,
            Basar = basar,
            SellerId = seller.Id,
            Seller = seller,
        };
        ProductEntity product = _dataGeneratorService.NextProduct(_dataGeneratorService.NextBrand(), _dataGeneratorService.NextProductType(), acceptSession);
        if (valueState.HasValue)
        {
            product.ValueState = valueState.Value;
        }
        if (storageState.HasValue)
        {
            product.StorageState = storageState.Value;
        }
        return product;
    }

    private static TransactionEntity NextTransaction(TransactionType transactionType, BasarEntity basar, SellerEntity? seller, IReadOnlyList<ProductEntity> products)
    {
        TransactionEntity transaction = new()
        {
            Basar = basar,
            BasarId= basar.Id,
            Id = RandomExtensions.GetPositiveInt32(),
            Type = transactionType,
            Notes = "Bla Bla Bla Anmerkung...",
            Number = RandomExtensions.GetPositiveInt32(),
            TimeStamp = new DateTime(2063, 04, 05, 15, 22, 11)
        };
        if (seller != null)
        {
            transaction.Seller = seller;
            transaction.SellerId = seller.Id;
        }
        foreach (ProductEntity product in products)
        {
            ProductToTransactionEntity productToTransaction = new()
            {
                Product = product,
                ProductId = product.Id,
                Transaction = transaction,
                TransactionId = transaction.Id,
            };
            transaction.Products.Add(productToTransaction);
        }

        return transaction;
    }
}
