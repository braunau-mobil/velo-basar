using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloBasarContext : IdentityDbContext
    {
        private const string NextNumberSql = "update \"Number\" set \"Value\"=\"Value\" + 1 where \"BasarId\" = @BasarId and \"Type\" = @Type;select \"Value\" from \"Number\"  where \"BasarId\" = @BasarId and \"Type\" = @Type";
        private const string PdfContentType = "application/pdf";

        private readonly PdfCreator _pdfCreator = new PdfCreator();

        private readonly IStringLocalizer<SharedResource> _localizer;

        public VeloBasarContext (DbContextOptions<VeloBasarContext> options, IStringLocalizer<SharedResource> localizer)
            : base(options)
        {
            _localizer = localizer;
        }

        public DbSet<Basar> Basar { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<FileStore> FileStore { get; set; }
        public DbSet<Number> Number { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Settings> SettingsSet { get; set; }
        public DbSet<ProductsTransaction> Transactions { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public Settings Settings
        {
            get
            {
                return SettingsSet
                    .Include(s => s.ActiveBasar)
                    .First();
            }
        }

        public async Task<ProductsTransaction> AcceptProductsAsync(Basar basar, int sellerId, IList<Product> products)
        {
            var productsInserted = await InsertProductsAsync(products);

            var tx = await CreateTransactionAsync(basar, TransactionType.Acceptance, sellerId, productsInserted.ToArray());
            await GenerateTransactionDocumentAsync(tx);

            return tx;
        }
        public async Task<ProductsTransaction> CancelProductsAsync(Basar basar, int saleId, IList<Product> products)
        {
            if (!products.IsAllowed(TransactionType.Cancellation))
            {
                return null;
            }

            products.SetState(TransactionType.Cancellation);
            await SaveChangesAsync();

            var sale = await Transactions.GetAsync(saleId);
            await RemoveProductsFromTransactionAsync(sale, products);

            return await CreateTransactionAsync(basar, TransactionType.Cancellation, products.ToArray());
        }
        public  async Task<bool> CanDeleteBasarAsync(Basar basar)
        {
            return !await Transactions.AnyAsync(t => t.BasarId == basar.Id);
        }
        public async Task<bool> CanDeleteBrandAsync(Brand item)
        {
            return !await Product.AnyAsync(p => p.BrandId == item.Id);
        }
        public async Task<bool> CanDeleteProductTypeAsync(ProductType item)
        {
            return !await Product.AnyAsync(p => p.TypeId == item.Id);
        }
        public async Task<ProductsTransaction> CheckoutProductsAsync(Basar basar, IList<int> productIds)
        {
            var products = Product.GetMany(productIds);
            return await DoTransactionAsync(basar, TransactionType.Sale, null, await products.ToArrayAsync());
        }
        public async Task<Brand> CreateBrand(Brand brand)
        {
            await Brand.AddAsync(brand);
            await SaveChangesAsync();
            return brand;
        }
        public async Task<Basar> CreateBasarAsync(Basar basar)
        {
            await Basar.AddAsync(basar);
            await SaveChangesAsync();

            foreach (var enumValue in Enum.GetValues(typeof(TransactionType)))
            {
                await CreateNewNumberAsync(basar, (TransactionType)enumValue);
            }

            return basar;
        }
        public async Task<FileStore> CreateLabelsForAcceptanceAsync(Basar basar, int acceptanceNumber)
        {
            var acceptance = await Transactions.GetAsync(basar, TransactionType.Acceptance, acceptanceNumber);
            return await CreateLabelsAndCombineToOnePdfAsync(basar, acceptance.Products.Select(pt => pt.Product));
        }
        public async Task<FileStore> CreateLabelsForSellerAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).Where(p => p.Label != null).ToListAsync();
            return await CreateLabelsAndCombineToOnePdfAsync(basar, products);
        }
        public async Task<Basar> CreateNewBasarAsync(DateTime date, string name, decimal productCommission, decimal productDiscount, decimal sellerDiscount)
        {
            var basar = new Basar
            {
                Date = date,
                Name = name,
                ProductCommission = productCommission,
                ProductDiscount = productDiscount,
                SellerDiscount = sellerDiscount
            };
            return await CreateBasarAsync(basar);
        }
        public async Task<ProductType> CreateProductType(ProductType productType)
        {
            await ProductTypes.AddAsync(productType);
            await SaveChangesAsync();
            return productType;
        }
        public async Task DeleteBasarAsync(int basarId)
        {
            var basar = await Basar.GetAsync(basarId);
            if (basar != null && !await CanDeleteBasarAsync(basar))
            {
                throw new InvalidOperationException(_localizer["Basar mit ID={0} kann nicht gelöscht werden.", basar.Id]);
            }

            foreach (var enumValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (TransactionType)enumValue;
                var number = await Number.FirstAsync(n => n.BasarId == basar.Id && n.Type == transactionType);
                Number.Remove(number);
            }

            Basar.Remove(basar);
            await SaveChangesAsync();
            
        }
        public async Task DeleteBrand(int brandId)
        {
            var brand = await Brand.GetAsync(brandId);
            if (brand != null)
            {
                Brand.Remove(brand);
                await SaveChangesAsync();
            }
        }
        public async Task DeleteProductType(int id)
        {
            var productType = await ProductTypes.GetAsync(id);
            if (productType != null)
            {
                ProductTypes.Remove(productType);
                await SaveChangesAsync();
            }
        }
        public async Task<ProductsTransaction> DoTransactionAsync(Basar basar, TransactionType transactionType, string notes, params Product[] products)
        {
            if (!products.IsAllowed(transactionType))
            {
                return null;
            }

            products.SetState(transactionType);
            await SaveChangesAsync();

            return await CreateTransactionAsync(basar, transactionType, null, notes, products);
        }
        public async Task<BasarStatistic> GetBasarStatisticAsnyc(int basarId)
        {
            var basar = await Basar.GetAsync(basarId);
            var products = await GetProductsForBasar(basar).ToArrayAsync();

            //  @todo kinda hack, find better place to create the stats....
            return new BasarStatistic
            {
                Basar = basar,
                Sales = new PieChartData
                {
                    BackgroundColors = new[]
                    {
                        "'rgb(40, 167, 69)'",
                        "'rgb(255, 193, 7)'",
                        "'rgb(220, 53, 69)'",
                        "'rgb(220, 53, 69)'"
                    },
                    DataPoints = new[]
                    {
                        products.Count(x => x.StorageState == StorageState.Available),
                        products.Count(x => x.StorageState == StorageState.Sold),
                        products.Count(x => x.StorageState == StorageState.Gone),
                        products.Count(x => x.StorageState == StorageState.Locked)
                    },
                    Labels = new[]
                    {
                        _localizer["Verfügbar"].Value,
                        _localizer["Verkauft"].Value,
                        _localizer["Verschwunden"].Value,
                        _localizer["Gesperrt"].Value,
                    }
                }                
            };
        }
        public IQueryable<Product> GetProductsForBasar(Basar basar)
        {
            return Transactions.GetMany(TransactionType.Acceptance, basar).SelectMany(a => a.Products).Select(pa => pa.Product).IncludeAll();
        }
        public IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId)
        {
            return Transactions.GetMany(TransactionType.Acceptance, basar, sellerId).SelectMany(a => a.Products).Select(pa => pa.Product).IncludeAll();
        }
        public async Task<SellerStatistics> GetSellerStatisticsAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).ToArrayAsync();
            var soldProducts = products.WhereStorageState(StorageState.Sold).ToArray();
            return new SellerStatistics
            {
                AceptedProductCount = products.Length,
                SettlementAmout = soldProducts.Sum(p => p.Price),
                NotSoldProductCount = products.WhereStorageStateIsNot(StorageState.Sold).Count(),
                PickedUpProductCount = products.Where(p => p.StorageState == StorageState.Gone && p.ValueState == ValueState.Settled).Count(),
                SoldProductCount = soldProducts.Length
            };
        }
        public async Task<int> GetTransactionNumberForProductAsync(Basar basar, TransactionType type, int productId)
        {
            var transaction = await Transactions.AsNoTracking().Include(t => t.Products).Where(t => t.Type == type && t.BasarId == basar.Id).FirstOrDefaultAsync(t => t.Products.Any(pt => pt.ProductId == productId));
            return transaction.Number;
        }
        public async Task<TransactionStatistic[]> GetTransactionStatistics(TransactionType transactionType, Basar basar, int sellerId)
        {
            return await Transactions.GetMany(transactionType, basar, sellerId).AsNoTracking().Select(a => new TransactionStatistic
            {
                Transaction = a,
                ProductCount = a.Products.Count,
                Amount = a.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }
        public async Task InitializeDatabase(UserManager<IdentityUser> userManager, InitializationConfiguration config)
        {
            await Database.EnsureCreatedAsync();

            var adminUser = new IdentityUser
            {
                Email = config.AdminUserEMail,
                UserName = config.AdminUserEMail
            };
            await userManager.CreateAsync(adminUser, "root");

            var settings = new Settings
            {
                IsInitialized = true
            };
            await SettingsSet.AddAsync(settings);
            await SaveChangesAsync();
        }
        public bool IsInitialized()
        {
            try
            {
                return Settings.IsInitialized;
            }
            catch
            {
                return false;
            }
        }
        public int NextNumber(Basar basar, TransactionType transactionType)
        {
            var number = -1;

            //  @todo i was ned wieso, aber wann i de drecks Connection in using pack, dann krachts da gewaltig
            var connection = Database.GetDbConnection() as NpgsqlConnection;
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = NextNumberSql;
                command.Parameters.AddWithValue("@BasarId", basar.Id);
                command.Parameters.AddWithValue("@Type", (int)transactionType);

                number = (int)command.ExecuteScalar();
            }

            connection.Close();

            return number;
        }
        public async Task<ProductsTransaction> SettleSellerAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).Where(p => p.IsAllowed(TransactionType.Settlement)).ToListAsync();
            products.SetState(TransactionType.Settlement);

            return await CreateTransactionAsync(basar, TransactionType.Settlement, sellerId, products.ToArray());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductToTransaction>().HasKey(ap => new { ap.ProductId, ap.TransactionId });
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
        }

        private async Task CreateNewNumberAsync(Basar basar, TransactionType type)
        {
            var number = new Number
            {
                Basar = basar,
                Value = 0,
                Type = type
            };
            await Number.AddAsync(number);
            await SaveChangesAsync();
        }
        private async Task<ProductsTransaction> CreateTransactionAsync(Basar basar, TransactionType transactionType, params Product[] products)
        {
            return await CreateTransactionAsync(basar, transactionType, null, null, products);
        }
        private async Task<ProductsTransaction> CreateTransactionAsync(Basar basar, TransactionType transactionType, int? sellerId, params Product[] products)
        {
            return await CreateTransactionAsync(basar, transactionType, sellerId, null, products);
        }
        private async Task<ProductsTransaction> CreateTransactionAsync(Basar basar, TransactionType transactionType, int? sellerId, string notes, params Product[] products)
        {
            var tx = new ProductsTransaction
            {
                BasarId = basar.Id,
                Number = NextNumber(basar, transactionType),
                TimeStamp = DateTime.Now,
                Notes = notes,
                Type = transactionType,
                SellerId = sellerId
            };
            tx.Products = products.Select(p => new ProductToTransaction { Product = p, Transaction = tx }).ToList();
            await Transactions.AddAsync(tx);
            await SaveChangesAsync();

            await GenerateTransactionDocumentAsync(tx);

            return tx;
        }
        private async Task<FileStore> CreateLabelsAndCombineToOnePdfAsync(Basar basar, IEnumerable<Product> products)
        {
            var files = new List<byte[]>();
            foreach (var product in products)
            {
                var file = await CreateLabelAsync(basar, product);
                files.Add(file.Data);
            }

            if (files.Count <= 0)
            {
                return null;
            }

            return new FileStore
            {
                Data = _pdfCreator.Combine(files),
                ContentType = PdfContentType
            };
        }
        private async Task<FileStore> CreateLabelAsync(Basar basar, Product product)
        {
            var fileStore = new FileStore
            {
                ContentType = PdfContentType,
                Data = _pdfCreator.CreateLabel(basar, product)
            };
            await FileStore.AddAsync(fileStore);
            await SaveChangesAsync();

            product.Label = fileStore.Id;

            await SaveChangesAsync();

            return fileStore;
        }
        private async Task<FileStore> GenerateTransactionDocumentAsync(ProductsTransaction transaction)
        {
            FileStore fileStore;
            if (transaction.DocumentId == null)
            {
                fileStore = new FileStore
                {
                    ContentType = PdfContentType
                };
                await FileStore.AddAsync(fileStore);
                await SaveChangesAsync();

                transaction.DocumentId = fileStore.Id;
            }
            else
            {
                fileStore = await FileStore.GetAsync(transaction.DocumentId.Value);
            }

            if (transaction.Type == TransactionType.Acceptance)
            {
                fileStore.Data = _pdfCreator.CreateAcceptance(transaction);
            }
            else if (transaction.Type == TransactionType.Sale)
            {
                fileStore.Data = _pdfCreator.CreateSale(transaction);
            }
            else if (transaction.Type == TransactionType.Settlement)
            {
                fileStore.Data = _pdfCreator.CreateSettlement(transaction);
            }

            await SaveChangesAsync();

            return fileStore;
        }
        private async Task<IList<Product>> InsertProductsAsync(IList<Product> products)
        {
            var newProducts = new List<Product>();
            foreach (var product in products)
            {
                var newProduct = new Product
                {
                    BrandId = product.BrandId,
                    Color = product.Color,
                    Description = product.Description,
                    FrameNumber = product.FrameNumber,
                    Label = product.Label,
                    Price = product.Price,
                    StorageState = StorageState.Available,
                    TireSize = product.TireSize,
                    TypeId = product.TypeId,
                    ValueState = ValueState.NotSettled
                };

                if (product.Brand != null)
                {
                    newProduct.Brand = product.Brand;
                    newProduct.BrandId = product.BrandId;
                }

                if (product.Type != null)
                {
                    newProduct.Type = product.Type;
                    newProduct.TypeId = product.TypeId;
                }

                newProducts.Add(newProduct);
            }

            await Product.AddRangeAsync(newProducts);
            await SaveChangesAsync();

            return newProducts;
        }
        private async Task RemoveProductsFromTransactionAsync(ProductsTransaction transaction, IList<Product> productsToRemove)
        {
            foreach (var product in productsToRemove)
            {
                transaction.Products.Remove(transaction.Products.First(s => s.ProductId == product.Id));
            }
            await SaveChangesAsync();
        }
    }
}

