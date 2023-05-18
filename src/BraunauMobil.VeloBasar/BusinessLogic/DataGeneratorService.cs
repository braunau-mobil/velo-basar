using BraunauMobil.VeloBasar.Crud;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Cryptography;
using System.Text;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class DataGeneratorService
    : IDataGeneratorService
{
    private readonly VeloDbContext _db;
    private readonly ISetupService _setupService;
    private readonly ISellerService _sellerService;
    private readonly IAcceptSessionService _acceptSessionService;
    private readonly IAcceptProductService _acceptProductService;
    private readonly ITransactionService _transactionService;
    private readonly BasarCrudService _basarCrudService;
    private readonly IClock _clock;
    private DataGeneratorConfiguration? _config;

    public DataGeneratorService(VeloDbContext db, ISetupService setupService, BasarCrudService basarCrudService, ISellerService sellerService, IAcceptSessionService acceptSessionService, IAcceptProductService acceptProductService, ITransactionService transactionService, IClock clock)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _setupService = setupService ?? throw new ArgumentNullException(nameof(setupService));
        _basarCrudService = basarCrudService ?? throw new ArgumentNullException(nameof(basarCrudService));
        _sellerService = sellerService ?? throw new ArgumentNullException(nameof(sellerService));
        _acceptSessionService = acceptSessionService ?? throw new ArgumentNullException(nameof(acceptSessionService));
        _acceptProductService = acceptProductService ?? throw new ArgumentNullException(nameof(acceptProductService));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

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

    public void Contextualize(DataGeneratorConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        _config = config;
    }

    public async Task DropDatabaseAsync()
    {
        await _db.Database.EnsureDeletedAsync();
        await _db.SaveChangesAsync();
    }

    public async Task GenerateAsync()
    {
        //  Detach everything because otherwise this could lead to problems when creating new data
        foreach (EntityEntry entry in _db.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        await _db.Database.EnsureDeletedAsync();
        await _db.SaveChangesAsync();

        await _setupService.CreateDatabaseAsync();
        await _db.SaveChangesAsync();

        await _setupService.InitializeDatabaseAsync(Config);
        await _db.SaveChangesAsync();

        for (int basarNumber = 1; basarNumber <= Config.BasarCount; basarNumber++)
        {
            BasarEntity basar = await CreateBasarAsync(Config.FirstBasarDate.AddYears(basarNumber - 1), $"{basarNumber}. Fahrradbasar");
            if (basarNumber == 1)
            {
                basar.State = ObjectState.Enabled;
            }

            if (Config.SimulateSales && basarNumber < Config.BasarCount)
            {
                await SettleSellers(basar.Id);
            }
        }

        await _db.SaveChangesAsync();
    }

    public BasarEntity NextBasar()
        => new()
        {
            Id = RandomExtensions.GetPositiveInt32(),
            CreatedAt = _clock.GetCurrentDateTime(),
            Date = new DateTime(2063, 04, 05),
            Location = Names.Cities.GetRandomElement(),
            Name = $"Basar #{RandomExtensions.GetPositiveInt32()}",
            ProductCommissionPercentage = RandomNumberGenerator.GetInt32(5, 21),
            State = ObjectState.Enabled,
            UpdatedAt = _clock.GetCurrentDateTime(),
        };

    public BrandEntity NextBrand()
        => new()
        {
            Id = RandomExtensions.GetPositiveInt32(),
            CreatedAt = _clock.GetCurrentDateTime(),
            Name = $"Basar #{RandomExtensions.GetPositiveInt32()}",
            State = ObjectState.Enabled,
            UpdatedAt = _clock.GetCurrentDateTime(),
        };

    public CountryEntity NextCountry()
        => new()
        {
            Id = RandomExtensions.GetPositiveInt32(),
            CreatedAt = _clock.GetCurrentDateTime(),
            Name = "Mittelerde",
            Iso3166Alpha3Code = "MEA",
            State = ObjectState.Enabled,
            UpdatedAt = _clock.GetCurrentDateTime(),
        };

    public ProductEntity NextProduct(BrandEntity brand, ProductTypeEntity productType, AcceptSessionEntity session)
    {
        ArgumentNullException.ThrowIfNull(session);

        return new()
        {
            Brand = brand,
            Color = Names.Colors.GetRandomElement(),
            Description = $"Beschreibung für Produkt",
            FrameNumber = Guid.NewGuid().ToString(),
            Price = NextPrice(),
            Session = session,
            SessionId = session.Id,
            StorageState = StorageState.NotAccepted,
            ValueState = ValueState.NotSettled,
            TireSize = Names.TireSizes.GetRandomElement(),
            Type = productType
        };
    }

    public ProductTypeEntity NextProductType()
        => new()
        {
            Id = RandomExtensions.GetPositiveInt32(),
            CreatedAt = _clock.GetCurrentDateTime(),
            Name = $"Basar #{RandomExtensions.GetPositiveInt32()}",
            State = ObjectState.Enabled,
            UpdatedAt = _clock.GetCurrentDateTime(),
        };

    public SellerEntity NextSeller(CountryEntity country)
        => new()
        {
            FirstName = Names.FirstNames.GetRandomElement(),
            LastName = Names.FirstNames.GetRandomElement(),
            Country = country,
            City = Names.Cities.GetRandomElement(),
            Street = $"{Names.Streets.GetRandomElement()} {RandomNumberGenerator.GetInt32(1, 50)}",
            PhoneNumber = NextPhoneNumber(),
            EMail = $"{RandomExtensions.GetPositiveInt32()}@domain.com",
            HasNewsletterPermission = RandomExtensions.GetBool(),
            ZIP = $"{RandomNumberGenerator.GetInt32(1, 10)}{RandomNumberGenerator.GetInt32(1, 10)}{RandomNumberGenerator.GetInt32(1, 10)}{RandomNumberGenerator.GetInt32(1, 10)}",
            State = ObjectState.Enabled
        };

    private async Task<BasarEntity> CreateBasarAsync(DateTime date, string name)
    {
        BasarEntity basar = NextBasar();
        basar.Date = date;
        basar.Name = name;
        basar.Id = 0;
        int basarId = await _basarCrudService.CreateAsync(basar);

        int sellerCount = RandomNumberGenerator.GetInt32(Config.MinSellers, Config.MaxSellers + 1);
        for (int sellerNumber = 1; sellerNumber <= sellerCount; sellerNumber++)
        {
            await CreateSellerWithAcceptancesAsync(basarId);
        }

        if (Config.SimulateSales)
        {
            await SimulateSalesAsync(basarId);
        }

        return basar;
    }

    private async Task SettleSellers(int basarId)
    {
        IReadOnlyList<int?> sellerIds = await _db.Transactions
            .Where(transaction => transaction.BasarId == basarId && transaction.SellerId.HasValue)
            .Select(Transaction => Transaction.SellerId)
            .Distinct()
            .ToArrayAsync();

        foreach (int? sellerId in sellerIds)
        {
            if (!sellerId.HasValue)
            {
                continue;
            }

            IReadOnlyList<int> productIds = await _db.Products
                .Include(product => product.Session)
                .WhereBasarAndSeller(basarId, sellerId.Value)
                .Select(product => product.Id)
                .ToArrayAsync();

            await _transactionService.SettleAsync(basarId, sellerId.Value, productIds);
        }
    }

    private async Task SimulateSalesAsync(int basarId)
    {
        IReadOnlyList<int> productIds = await _db.Products
            .Include(product => product.Session)
            .Where(product => product.Session.BasarId == basarId)
            .Select(product => product.Id)
            .ToArrayAsync();
        Queue<int> productIdsForSale = new(productIds.Take(RandomNumberGenerator.GetInt32(productIds.Count / 3, productIds.Count + 1)));
        while (productIdsForSale.Count > 0)
        {
            int productCountForSale = RandomNumberGenerator.GetInt32(1, 5);
            List<int> productIdsForSaleTransaction = new ();
            while (productCountForSale > 0)
            {
                int productId = productIds.GetRandomElement();
                productIdsForSaleTransaction.Add(productIdsForSale.Dequeue());
                productCountForSale--;
            }

            await _transactionService.CheckoutAsync(basarId, productIdsForSaleTransaction);
        }
    }

    private async Task CreateSellerWithAcceptancesAsync(int basarId)
    {
        SellerEntity seller = NextSeller(_db.Countries.GetRandomElement());
        seller.Id = 0;
        await _sellerService.CreateAsync(seller);

        int acceptancePerCustomerCount = RandomNumberGenerator.GetInt32(Config.MinAcceptancesPerSeller, Config.MaxAcceptancesPerSeller + 1);
        while (acceptancePerCustomerCount > 0)
        {
            await CreateAcceptanceAsync(basarId, seller);
            acceptancePerCustomerCount--;
        }
    }

    private async Task CreateAcceptanceAsync(int basarId, SellerEntity seller)
    {
        AcceptSessionEntity session = await _acceptSessionService.CreateAsync(basarId, seller.Id);

        int productCount = NextProductCount();
        foreach (int _ in Enumerable.Range(0, productCount))
        {
            ProductEntity product = NextProduct(session);
            await _acceptProductService.CreateAsync(product);
        }

        await _acceptSessionService.SubmitAsync(session.Id);
    }

    private ProductEntity NextProduct(AcceptSessionEntity session)
        => NextProduct(_db.Brands.GetRandomElement(), _db.ProductTypes.GetRandomElement(), session);

    private static string NextPhoneNumber()
    {
        StringBuilder sb = new();
        for (int counter = 0; counter < RandomNumberGenerator.GetInt32(8, 11); counter++)
        {
            sb.Append(RandomNumberGenerator.GetInt32(0, 10));
        }
        return sb.ToString();
    }

    private int NextProductCount()
         => Math.Max((int)RandomExtensions.GetGaussian(Config.MeanProductsPerSeller, Config.StdDevProductsPerSeller), 1);

    private decimal NextPrice()
        => Math.Round((decimal)RandomExtensions.GetGaussian((double)Config.MeanPrice, (double)Config.StdDevPrice), 2);
}
