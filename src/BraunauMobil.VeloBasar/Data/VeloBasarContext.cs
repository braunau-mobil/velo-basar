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
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloBasarContext : IdentityDbContext
    {
        private const string NextNumberSql = "update \"Number\" set \"Value\"=\"Value\" + 1 where \"BasarId\" = @BasarId and \"Type\" = @Type;select \"Value\" from \"Number\"  where \"BasarId\" = @BasarId and \"Type\" = @Type";
        private const string PdfContentType = "application/pdf";

        private readonly PdfCreator _pdfCreator = new PdfCreator();

        public VeloBasarContext (DbContextOptions<VeloBasarContext> options)
            : base(options)
        {
        }

        public DbSet<Basar> Basar { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<FileStore> FileStore { get; set; }

        public DbSet<Number> Number { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Seller> Seller { get; set; }

        public DbSet<Settings> SettingsSet { get; set; }

        public DbSet<ProductsTransaction> Transactions { get; set; }

        public Settings Settings
        {
            get
            {
                return SettingsSet.First();
            }
        }

        public async Task<ProductsTransaction> AddProductToSaleAsync(Basar basar, int? saleId, Product product)
        {
            var sale = await CreateOrGetSaleAsync(basar, saleId);

            if (sale.Products == null)
            {
                sale.Products = new List<ProductToTransaction>();
            }
            sale.Products.Add(new ProductToTransaction
            {
                Product = product,
                Transaction = sale
            });

            product.StorageStatus = StorageStatus.Sold;

            await SaveChangesAsync();

            return sale;
        }

        public async Task<ProductsTransaction> AcceptProductsAsync(Basar basar, int sellerId, int? acceptanceId, params Product[] products)
        {
            ProductsTransaction acceptance;
            if (acceptanceId == null)
            {
                acceptance = new ProductsTransaction
                {
                    Type = TransactionType.Acceptance,
                    Basar = basar,
                    SellerId = sellerId,
                    TimeStamp = DateTime.Now,
                    Products = new List<ProductToTransaction>()
                };
            }
            else
            {
                acceptance = await Transactions.Include(a => a.Products).FirstAsync(a => a.Id == acceptanceId);
            }

            foreach (var product in products)
            {
                await Product.AddAsync(product);

                var productAcceptance = new ProductToTransaction
                {
                    Transaction = acceptance,
                    Product = product
                };
                acceptance.Products.Add(productAcceptance);
            }

            if (acceptanceId == null)
            {
                acceptance.Number = NextNumber(basar, TransactionType.Acceptance);
                await Transactions.AddAsync(acceptance);
            }

            await SaveChangesAsync();

            return acceptance;
        }

        public async Task CancelProductAsync(Basar basar, int productId)
        {
            var product = await GetProductAsync(productId);

            var cancellation = new ProductsTransaction
            {
                Type = TransactionType.Cancellation,
                Basar = basar,
                Number = NextNumber(basar, TransactionType.Cancellation),
                TimeStamp = DateTime.Now
            };
            cancellation.Products = new ProductToTransaction[]
            {
                new ProductToTransaction
                {
                    Transaction = cancellation,
                    Product = product
                }
            };
            product.StorageStatus = StorageStatus.Available;

            await SaveChangesAsync();
        }

        public async Task<ProductsTransaction> CreateOrGetSaleAsync(Basar basar, int? saleId)
        {
            if (saleId == null)
            {
                var sale = new ProductsTransaction
                {
                    Type = TransactionType.Sale,
                    Basar = basar,
                    Number = NextNumber(basar, TransactionType.Sale),
                    TimeStamp = DateTime.Now,
                    Products = new List<ProductToTransaction>()
                };
                Transactions.Add(sale);
                await SaveChangesAsync();
                return sale;
            }

            return await Transactions.FirstAsync(s => s.Id == saleId);
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
            await Basar.AddAsync(basar);
            await SaveChangesAsync();

            await CreateNewNumberAsync(basar, TransactionType.Acceptance);
            await CreateNewNumberAsync(basar, TransactionType.Settlement);
            await CreateNewNumberAsync(basar, TransactionType.Cancellation);
            await CreateNewNumberAsync(basar, TransactionType.Sale);

            return basar;
        }

        public async Task CreateNewNumberAsync(Basar basar, TransactionType type)
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

        public async Task GenerateLabel(Basar basar, Product product)
        {
            var fileStore = new FileStore
            {
                ContentType = PdfContentType,
                Data = _pdfCreator.CreateLabel(basar, product)
            };
            await FileStore.AddAsync(fileStore);
            await SaveChangesAsync();

            product.Label = fileStore.Id;
        }

        public async Task<FileStore> GenerateAcceptanceDocIfNotExistAsync(Basar basar, int acceptanceId)
        {
            var acceptance = await GetAcceptanceAsync(acceptanceId);

            FileStore fileStore;
            if (acceptance.DocumentId == null)
            {
                fileStore = new FileStore
                {
                    ContentType = PdfContentType,
                    Data = _pdfCreator.CreateAcceptance(basar, acceptance)
                };
                await FileStore.AddAsync(fileStore);
                await SaveChangesAsync();

                acceptance.DocumentId = fileStore.Id;
                await SaveChangesAsync();
            }
            else
            {
                fileStore = await GetFileAsync(acceptance.DocumentId.Value);
            }

            return fileStore;
        }

        public async Task GenerateMissingLabelsAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).Where(p => p.Label == null).ToListAsync();
            foreach (var product in products)
            {
                await GenerateLabel(basar, product);
            }

            await SaveChangesAsync();
        }

        public async Task<FileStore> GenerateSaleDocIfNotExistAsync(Basar basar, int saleId)
        {
            var sale = await GetSaleAsync(saleId);

            FileStore fileStore;
            if (sale.DocumentId == null)
            {
                fileStore = new FileStore
                {
                    ContentType = PdfContentType,
                    Data = _pdfCreator.CreateSale(basar, sale)
                };
                await FileStore.AddAsync(fileStore);
                await SaveChangesAsync();

                sale.DocumentId = fileStore.Id;
                await SaveChangesAsync();
            }
            else
            {
                fileStore = await GetFileAsync(sale.DocumentId.Value);
            }

            return fileStore;
        }

        public async Task<FileStore> GenerateSettlementDocIfNotExistAsync(Basar basar, ProductsTransaction settlement)
        {
            var fileStore = new FileStore
            {
                ContentType = PdfContentType,
                Data = _pdfCreator.CreateSettlement(basar, settlement)
            };
            await FileStore.AddAsync(fileStore);
            await SaveChangesAsync();

            settlement.DocumentId = fileStore.Id;
            await SaveChangesAsync();

            return fileStore;
        }

        public async Task<ProductsTransaction> GetAcceptanceAsync(int acceptanceId)
        {
            return await Transactions
                .Include(a => a.Seller)
                    .ThenInclude(s => s.Country)
                .Include(a => a.Products)
                    .ThenInclude(pa => pa.Product)
                .FirstAsync(a => a.Id == acceptanceId);
        }

        public IQueryable<ProductsTransaction> GetAcceptances(Basar basar, Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            return GetTransactions(basar, TransactionType.Acceptance, additionalPredicate);
        }

        public IQueryable<ProductsTransaction> GetAcceptancesForSeller(Basar basar, int sellerId)
        {
            return GetAcceptances(basar, t => t.SellerId == sellerId);
        }

        public async Task<TransactionStatistic[]> GetAcceptanceStatisticsAsync(Basar basar, int sellerId)
        {
            return await GetAcceptancesForSeller(basar, sellerId).Include(a => a.Products).AsNoTracking().Select(a => new TransactionStatistic
            {
                Transaction = a,
                ProductCount = a.Products.Count,
                Amount = a.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }

        public async Task<FileStore> GetAllLabelsAsyncAsCombinedPdf(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).Where(p => p.Label != null).ToListAsync();
            var files = new List<byte[]>();
            foreach (var product in products)
            {
                var file = await GetFileAsync(product.Label.Value);
                files.Add(file.Data);
            }

            var result = new FileStore
            {
                Data = _pdfCreator.Combine(files),
                ContentType = PdfContentType
            };
            return result;
        }

        public async Task<Basar> GetBasarAsync(int basarId)
        {
            return await Basar.FirstOrDefaultAsync(b => b.Id == basarId);
        }

        public IQueryable<ProductsTransaction> GetCacncellations(Basar basar, Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            return GetTransactions(basar, TransactionType.Cancellation, additionalPredicate);
        }

        public async Task<FileStore> GetFileAsync(int fileId)
        {
            return await FileStore.FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await Product.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public IQueryable<Product> GetProducts(string searchString = null)
        {
            var res = Product.OrderBy(p => p.Id);

            if (string.IsNullOrEmpty(searchString))
            {
                return res;
            }

            return res.Where(Expressions.ProductSearch(searchString));
        }

        public IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId)
        {
            return GetAcceptancesForSeller(basar, sellerId).SelectMany(a => a.Products).Select(pa => pa.Product);
        }

        public async Task<ProductsTransaction> GetSaleAsync(int saleId)
        {
            return await Transactions
                .Include(s => s.Products)
                    .ThenInclude(ps => ps.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);
        }

        public IQueryable<ProductsTransaction> GetSales(Basar basar, string searchString = null)
        {
            return GetTransactions(basar, TransactionType.Sale, Expressions.TransactionSearch(searchString));
        }

        public async Task<Seller> GetSellerAsync(int sellerId)
        {
            return await Seller.FirstOrDefaultAsync(b => b.Id == sellerId);
        }

        public IQueryable<Seller> GetSellers(string searchString = null)
        {
            var res = Seller
                .Include(s => s.Country).OrderBy(s => s.Id);

            if (string.IsNullOrEmpty(searchString))
            {
                return res;
            }

            return res.Where(Expressions.SellerSearch(searchString));
        }

        public IQueryable<Seller> GetSellers(string firstName, string lastName)
        {
            var res = Seller
                .Include(s => s.Country).OrderBy(s => s.Id);

            return res.Where(Expressions.SellerSearch(firstName, lastName));
        }

        public async Task<SellerStatistics> GetSellerStatisticsAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).ToArrayAsync();
            var soldProducts = products.WhereStorageState(StorageStatus.Sold).ToArray();
            return new SellerStatistics
            {
                AceptedProductCount = products.Length,
                SettlementAmout = soldProducts.Sum(p => p.Price),
                NotSoldProductCount = products.WhereStorageStateIsNot(StorageStatus.Sold).Count(),
                PickedUpProductCount = products.Where(p => p.StorageStatus == StorageStatus.Gone && p.ValueStatus == ValueStatus.Settled).Count(),
                SoldProductCount = soldProducts.Length
            };
        }

        public IQueryable<ProductsTransaction> GetSettlements(Basar basar, Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            return GetTransactions(basar, TransactionType.Settlement, additionalPredicate);
        }

        public IQueryable<ProductsTransaction> GetSettlementsForSeller(Basar basar, int sellerId)
        {
            return GetSettlements(basar, t => t.SellerId == sellerId);
        }

        public async Task<TransactionStatistic[]> GetSettlementStatisticsAsync(Basar basar, int sellerId)
        {
            return await GetSettlementsForSeller(basar, sellerId).Include(s => s.Products).AsNoTracking().Select(s => new TransactionStatistic
            {
                Transaction = s,
                ProductCount = s.Products.Count,
                Amount = s.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }

        public IQueryable<ProductsTransaction> GetTransactions(Basar basar, TransactionType type , Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            var result = Transactions.Where(t => t.Type == type && t.Basar == basar);

            if (additionalPredicate != null)
            {
                return result.Where(additionalPredicate);
            }

            return result;
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
            if (basar == null)
            {
                throw new ArgumentException("Invalid basar");
            }

            var number = -1;
            
            //  @todo i was ned wieso, aber wann i de drecks Connection in using pack, dann krachts da gewaltig
            var connection = Database.GetDbConnection() as NpgsqlConnection;
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = NextNumberSql;
                command.Parameters.AddWithValue("@BasarId", basar.Id);
                command.Parameters.AddWithValue("@Type", (int)transactionType);

                number = (int) command.ExecuteScalar();
            }

            connection.Close();

            return number;
        }

        public async Task<ProductsTransaction> SettleSellerAsync(Basar basar, int sellerId)
        {
            var settlement = new ProductsTransaction
            {
                Type = TransactionType.Settlement,
                Basar = basar,
                SellerId = sellerId,
                TimeStamp = DateTime.Now,
                Products = new List<ProductToTransaction>()
            };

            var products = await GetProductsForSeller(basar, sellerId).ToArrayAsync();
            foreach (var product in products)
            {
                if (product.StorageStatus == StorageStatus.Available)
                {
                    product.StorageStatus = StorageStatus.Gone;
                }
                else if (product.StorageStatus == StorageStatus.Sold)
                {
                    product.StorageStatus = StorageStatus.Gone;
                }
                product.ValueStatus = ValueStatus.Settled;

                var productSettlement = new ProductToTransaction
                {
                    Product = product,
                    Transaction = settlement
                };
                settlement.Products.Add(productSettlement);
            }

            settlement.Number = NextNumber(basar, TransactionType.Settlement);
            await Transactions.AddAsync(settlement);

            await SaveChangesAsync();

            return settlement;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductToTransaction>().HasKey(ap => new { ap.ProductId, ap.TransactionId});
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
        }
    }
}
