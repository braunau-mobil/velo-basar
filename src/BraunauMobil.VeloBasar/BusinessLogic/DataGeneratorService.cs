using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using Xan.Extensions;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class DataGeneratorService(VeloDbContext db, ISetupService setupService, IBasarService basarService, ISellerService sellerService, IAcceptSessionService acceptSessionService, IAcceptProductService acceptProductService, ITransactionService transactionService, IClock clock)
    : IDataGeneratorService
{
    private DataGeneratorConfiguration? _config;
    private Random? _random;
    private DateTime _timeStamp;

    private DataGeneratorConfiguration Config
    {
        get
        {
            if (_config == null)
            {
                throw new InvalidOperationException($"Please call {nameof(Contextualize)} first.");
            }
            return _config;
        }
    }

    private Random Random
    {
        get
        {
            if (_random == null)
            {
                throw new InvalidOperationException($"Please call {nameof(Contextualize)} first.");
            }

            return _random;
        }
    }

    public void Contextualize(DataGeneratorConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        _config = config;
        if (_config.Seed >= 0)
        {
            _random = new Random(_config.Seed);
        }
        else
        {
            _random = new Random();
        }
    }

    public async Task CreateDatabaseAsync()
    {
        await db.CreateDatabaseAsync();
    }

    public async Task DropDatabaseAsync()
    {
        await db.DropDatabaseAsync();
    }

    public async Task GenerateAsync()
    {
        //  Detach everything because otherwise this could lead to problems when creating new data
        foreach (EntityEntry entry in db.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        await DropDatabaseAsync();

        await CreateDatabaseAsync();

        await setupService.InitializeDatabaseAsync(Config);
        await db.SaveChangesAsync();

        for (int basarNumber = 1; basarNumber <= Config.BasarCount; basarNumber++)
        {
            BasarEntity basar = await CreateBasarAsync(Config.FirstBasarDate.AddYears(basarNumber - 1), $"{basarNumber}. Fahrradbasar");
            if (basarNumber == 1)
            {
                basar.State = ObjectState.Enabled;
            }

            if (Config.SimulateBasar && basarNumber < Config.BasarCount)
            {
                await SettleSellers(basar.Id);
            }
        }

        await db.SaveChangesAsync();
    }

    public BasarEntity NextBasar()
        => new()
        {
            Id = Random.Next(),
            CreatedAt = clock.GetCurrentDateTime(),
            Date = new DateTime(2063, 04, 05),
            Location = Random.GetRandomElement(Names.Cities),
            Name = $"Basar #{Random.Next()}",
            ProductCommissionPercentage = Random.Next(5, 21),
            State = ObjectState.Enabled,
            UpdatedAt = clock.GetCurrentDateTime(),
        };

    public string NextBrand()
        => Random.GetRandomElement(Names.BrandNames);

    public CountryEntity NextCountry()
        => new()
        {
            Id = Random.Next(),
            CreatedAt = clock.GetCurrentDateTime(),
            Name = "Mittelerde",
            Iso3166Alpha3Code = "MEA",
            State = ObjectState.Enabled,
            UpdatedAt = clock.GetCurrentDateTime(),
        };

    public ProductEntity NextProduct(string brand, ProductTypeEntity productType, AcceptSessionEntity session)
    {
        ArgumentNullException.ThrowIfNull(session);

        return new()
        {
            Brand = brand,
            Color = Random.GetRandomElement(Names.Colors),
            Description = $"Beschreibung für Produkt",
            FrameNumber = $"Rahmennummer #{Random.Next()}",
            Price = NextPrice(),
            Session = session,
            SessionId = session.Id,
            StorageState = StorageState.NotAccepted,
            ValueState = ValueState.NotSettled,
            TireSize = Random.GetRandomElement(Names.TireSizes),
            Type = productType
        };
    }

    public ProductTypeEntity NextProductType()
        => new()
        {
            Id = Random.Next(),
            CreatedAt = clock.GetCurrentDateTime(),
            Name = $"Basar #{Random.Next()}",
            State = ObjectState.Enabled,
            UpdatedAt = clock.GetCurrentDateTime(),
        };

    public SellerEntity NextSeller(CountryEntity country)
        => new()
        {
            FirstName = Random.GetRandomElement(Names.FirstNames),
            LastName = Random.GetRandomElement(Names.FirstNames),
            Country = country,
            City = Random.GetRandomElement(Names.Cities),
            Street = $"{Random.GetRandomElement(Names.Streets)} {Random.Next(1, 50)}",
            PhoneNumber = NextPhoneNumber(),
            EMail = $"{Random.Next()}@domain.com",
            HasNewsletterPermission = Random.GetBool(),
            ZIP = $"{Random.Next(1, 10)}{Random.Next(1, 10)}{Random.Next(1, 10)}{Random.Next(1, 10)}",
            State = ObjectState.Enabled
        };

    private async Task<BasarEntity> CreateBasarAsync(DateTime date, string name)
    {
        BasarEntity basar = NextBasar();
        basar.Date = date;
        basar.Name = name;
        basar.Id = 0;
        int basarId = await basarService.CreateAsync(basar);

        int sellerCount = Random.Next(Config.MinSellers, Config.MaxSellers + 1);
        for (int sellerNumber = 1; sellerNumber <= sellerCount; sellerNumber++)
        {
            await CreateSellerWithAcceptancesAsync(basarId);
        }

        if (Config.SimulateBasar)
        {
            await SimulateBasarAsync(basarId);
        }

        return basar;
    }

    private async Task SettleSellers(int basarId)
    {
        IList<int?> sellerIds = await db.Transactions
            .Where(transaction => transaction.BasarId == basarId && transaction.SellerId.HasValue)
            .Select(Transaction => Transaction.SellerId)
            .Distinct()
            .ToArrayAsync();
        Random.Shuffle(sellerIds);

        foreach (int? sellerId in sellerIds)
        {
            if (!sellerId.HasValue)
            {
                continue;
            }

            await sellerService.SettleAsync(basarId, sellerId.Value);
        }
    }

    private async Task SimulateBasarAsync(int basarId)
    {
        DateTime timeStampBeforeSales = _timeStamp;
        await SimulateSalesAsync(basarId, 45, 55);
        DateTime timeStampAfterSales = _timeStamp;

        _timeStamp = timeStampBeforeSales;
        await SimulateLossesAsync(basarId, 5, 10);
        _timeStamp = timeStampBeforeSales;
        await SimulateLocksAsync(basarId, 5, 10);

        _timeStamp = timeStampAfterSales;
        await SimulateSettlementsAsync(basarId);
    }

    private async Task SimulateSalesAsync(int basarId, int minPercentage, int maxPercentage)
    {
        IList<int> availableProductIds = await GetAvailableProductIdsAsync(basarId);
        Random.Shuffle(availableProductIds);

        Queue<int> productIdsForSale = new(Random.TakePercentage(availableProductIds, minPercentage, maxPercentage));
        while (productIdsForSale.Count > 0)
        {
            int productCount = NextSaleProductCount();
            List<int> productIdsForSaleTransaction = new();
            while (productCount > 0 && productIdsForSale.Count > 0)
            {
                int productId = productIdsForSale.Dequeue();
                productIdsForSaleTransaction.Add(productId);
                productCount--;
            }

            int transactionId = await transactionService.CheckoutAsync(basarId, productIdsForSaleTransaction);
            await SetTransactionTimestamp(transactionId);
        }
    }

    private async Task SimulateLossesAsync(int basarId, int minPercentage, int maxPercentage)
    {
        IList<int> availableProductIds = await GetAvailableProductIdsAsync(basarId);
        Random.Shuffle(availableProductIds);

        Queue<int> productIdsToLoose = new(Random.TakePercentage(availableProductIds, minPercentage, maxPercentage));
        while (productIdsToLoose.Count > 0)
        {
            int productId = productIdsToLoose.Dequeue();
            int transactionId = await transactionService.SetLostAsync(basarId, "Product could not be found anymore", productId);
            await SetTransactionTimestamp(transactionId);
        }
    }

    private async Task SimulateLocksAsync(int basarId, int minPercentage, int maxPercentage)
    {
        IList<int> availableProductIds = await GetAvailableProductIdsAsync(basarId);
        Random.Shuffle(availableProductIds);

        Queue<int> productIdsToLock = new(Random.TakePercentage(availableProductIds, minPercentage, maxPercentage));
        while (productIdsToLock.Count > 0)
        {
            int productId = productIdsToLock.Dequeue();
            int transactionId = await transactionService.LockAsync(basarId, "Product is damaged", productId);
            await SetTransactionTimestamp(transactionId);
        }
    }

    private async Task SimulateSettlementsAsync(int basarId)
    {
        IList<int> sellerIds = await db.Sellers
            .Where(seller => seller.ValueState == ValueState.NotSettled)
            .Select(seller => seller.Id)
            .ToArrayAsync();
        Random.Shuffle(sellerIds);

        Queue<int> sellerIdsToSettle = new(Random.TakePercentage(sellerIds, 50, 60));
        while (sellerIdsToSettle.Count > 0)
        {
            int sellerId = sellerIdsToSettle.Dequeue();
            await sellerService.SettleAsync(basarId, sellerId);
        }
    }

    private async Task CreateSellerWithAcceptancesAsync(int basarId)
    {
        SellerEntity seller = NextSeller(Random.GetRandomElement(db.Countries));
        seller.Id = 0;
        await sellerService.CreateAsync(seller);

        int acceptancePerCustomerCount = Random.Next(Config.MinAcceptancesPerSeller, Config.MaxAcceptancesPerSeller + 1);
        while (acceptancePerCustomerCount > 0)
        {
            await CreateAcceptanceAsync(basarId, seller);
            acceptancePerCustomerCount--;
        }
    }

    private async Task CreateAcceptanceAsync(int basarId, SellerEntity seller)
    {
        AcceptSessionEntity session = await acceptSessionService.CreateAsync(basarId, seller.Id);

        int productCount = NextProductCount();
        foreach (int _ in Enumerable.Range(0, productCount))
        {
            ProductEntity product = NextProduct(session);
            await acceptProductService.CreateAsync(product);
        }

        int transactionId = await acceptSessionService.SubmitAsync(session.Id);
        await SetTransactionTimestamp(transactionId);
    }

    private ProductEntity NextProduct(AcceptSessionEntity session)
        => NextProduct(NextBrand(), Random.GetRandomElement(db.ProductTypes), session);

    private string NextPhoneNumber()
    {
        StringBuilder sb = new();
        for (int counter = 0; counter < Random.Next(8, 11); counter++)
        {
            sb.Append(Random.Next(0, 10));
        }
        return sb.ToString();
    }

    private int NextProductCount()
         => Math.Max((int)Random.GetGaussian(Config.MeanProductsPerSeller, Config.StdDevProductsPerSeller), 1);

    private decimal NextPrice()
        => Math.Round((decimal)Random.GetGaussian((double)Config.MeanPrice, (double)Config.StdDevPrice), 2);

    private int NextSaleProductCount()
    {
        int number = Random.Next(0, 100);
        if (number.IsInRange(98, 99))
        {
            return 5;
        }
        if (number.IsInRange(95, 97))
        {
            return 4;
        }
        if (number.IsInRange(90, 94))
        {
            return 3;
        }
        if (number.IsInRange(80, 89))
        {
            return 2;
        }
        return 1;
    }

    private async Task<IList<int>> GetAvailableProductIdsAsync(int basarId)
    {
        return await db.Products
            .Include(product => product.Session)
            .Where(product => product.Session.BasarId == basarId && product.StorageState == StorageState.Available)
            .Select(product => product.Id)
            .ToArrayAsync();
    }

    private async Task SetTransactionTimestamp(int transactionId)
    {
        TransactionEntity tx = await db.Transactions.FirstByIdAsync(transactionId);
        tx.TimeStamp = _timeStamp;
        await db.SaveChangesAsync();

        _timeStamp = _timeStamp.AddMinutes(Random.Next(3, 8));
    }
}
