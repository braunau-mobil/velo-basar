﻿using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IO;
using System.Text;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public partial class AdminService
    : IAdminService
{
    private readonly Random _random = new ();
    private readonly IDocumentService _documentService;
    private readonly IDataGeneratorService _dataGeneratorService;
    private readonly ITokenProvider _tokenProvider;
    private readonly ExportSettings _exportSettings;
    private readonly VeloDbContext _db;

    public AdminService(IDocumentService documentService, ITokenProvider tokenProvider, IDataGeneratorService dataGeneratorService, IOptions<ExportSettings> options, VeloDbContext db)
    {
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _dataGeneratorService = dataGeneratorService ?? throw new ArgumentNullException(nameof(dataGeneratorService));
        _dataGeneratorService.Contextualize(new DataGeneratorConfiguration());

        ArgumentNullException.ThrowIfNull(options);
        _exportSettings = options.Value;

        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<FileDataEntity> CreateSampleAcceptanceDocumentAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = NextSeller(country);
        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled)
        };
        products[0].DonateIfNotSold = true;
        products[1].DonateIfNotSold = false;
        products[2].DonateIfNotSold = false;

        TransactionEntity transaction = NextTransaction(TransactionType.Acceptance, basar, seller, products);
        return await CreateTransactionDocumentAsync(transaction);
    }

    public async Task<FileDataEntity> CreateSampleLabelsAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = NextSeller(country);
        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled),
            NextProduct(basar, seller, StorageState.Available, ValueState.NotSettled)
        };
        byte[] data = await _documentService.CreateLabelsAsync(products);
        return FileDataEntity.Pdf("Sample_Labels.pdf", data);
    }

    public async Task<FileDataEntity> CreateSampleSaleDocumentAsync()
    {
        CountryEntity country = _dataGeneratorService.NextCountry();
        SellerEntity seller = NextSeller(country);
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
        SellerEntity seller = NextSeller(country);        

        BasarEntity basar = _dataGeneratorService.NextBasar();
        ProductEntity[] products = new[]
        {
            NextProduct(basar, seller, StorageState.Available, ValueState.Settled),
            NextProduct(basar, seller, StorageState.Available, ValueState.Settled),
            NextProduct(basar, seller, StorageState.Sold, ValueState.Settled),
            NextProduct(basar, seller, StorageState.Sold, ValueState.Settled)
        };

        TransactionEntity transaction = NextTransaction(TransactionType.Settlement, basar, seller, products);
        return await CreateTransactionDocumentAsync(transaction);
    }

    public async Task<FileDataEntity> ExportSellersForNewsletterAsCsvAsync(DateTime? minPermissionTimestamp)
    {
        Encoding encoding = Encoding.GetEncoding(_exportSettings.EncodingName);
        CsvConfiguration csvConfig = new (CultureInfo.InvariantCulture)
        {
            Delimiter = _exportSettings.Delimiter,
            Encoding = encoding,
            NewLine = _exportSettings.NewLine,
            Quote = _exportSettings.QuoteChar
        };

        IQueryable<SellerEntity> iq = _db.Sellers
            .Include(seller => seller.Country)
            .AsNoTracking()
            .OrderBy(seller => seller.NewsletterPermissionTimesStamp)
            .Where(seller => seller.HasNewsletterPermission);
        if (minPermissionTimestamp.HasValue)
        {
            iq = iq.Where(seller => seller.NewsletterPermissionTimesStamp >= minPermissionTimestamp);
        }

        SellerEntity[] sellers = await iq.ToArrayAsync();

        using MemoryStream memoryStream = new();
        using StreamWriter streamWriter = new(memoryStream, encoding);
        using CsvWriter csvWriter = new(streamWriter, csvConfig);
        
        csvWriter.Context.RegisterClassMap<SellerNewsletterExportMap>();
        csvWriter.WriteRecords(sellers);
        await csvWriter.FlushAsync();
            
        return FileDataEntity.Csv("sellers.csv", memoryStream.ToArray());
    }

    private async Task<FileDataEntity> CreateTransactionDocumentAsync(TransactionEntity transaction)
    {
        byte[] data = await _documentService.CreateTransactionDocumentAsync(transaction);
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

    private TransactionEntity NextTransaction(TransactionType transactionType, BasarEntity basar, SellerEntity? seller, IReadOnlyList<ProductEntity> products)
    {
        TransactionEntity transaction = new()
        {
            Basar = basar,
            BasarId= basar.Id,
            Id = _random.Next(),
            Type = transactionType,
            Notes = "Bla Bla Bla Anmerkung...",
            Number = _random.Next(),
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

    private SellerEntity NextSeller(CountryEntity country)
    {
        SellerEntity seller = _dataGeneratorService.NextSeller(country);
        seller.Id = _random.Next();
        seller.Token = _tokenProvider.CreateToken(seller);
        seller.IBAN = "AT536616326924127723";

        return seller;
    }
}
