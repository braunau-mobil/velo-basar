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
                return SettingsSet.First();
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
        public async Task<ProductsTransaction> CheckoutProductsAsync(Basar basar, IList<int> productIds)
        {
            var products = await Product.GetManyAsync(productIds);
            return await DoTransactionAsync(basar, TransactionType.Sale, null, products.ToArray());
        }
        public async Task<ProductsTransaction> SettleSellerAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).Where(p => p.IsAllowed(TransactionType.Settlement)).ToListAsync();
            products.SetState(TransactionType.Settlement);

            return await CreateTransactionAsync(basar, TransactionType.Settlement, sellerId, products.ToArray());
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

                var x = command.ExecuteScalar();
                number = (int)command.ExecuteScalar();
            }

            connection.Close();

            return number;
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
                fileStore = await GetFileAsync(transaction.DocumentId.Value);
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
        private async Task RemoveProductsFromTransactionAsync(ProductsTransaction transaction, IList<Product> productsToRemove)
        {
            foreach (var product in productsToRemove)
            {
                transaction.Products.Remove(transaction.Products.First(s => s.ProductId == product.Id));
            }
            await SaveChangesAsync();
        }



        public async Task<bool> CanDeleteBrandAsync(Brand item)
        {
            return !await Product.AnyAsync(p => p.BrandId == item.Id);
        }
        public async Task<bool> CanDeleteProductTypeAsync(ProductType item)
        {
            return !await Product.AnyAsync(p => p.TypeId == item.Id);
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

            foreach (var enumValue in Enum.GetValues(typeof(TransactionType)))
            {
                await CreateNewNumberAsync(basar, (TransactionType)enumValue);
            }

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

        public async Task DeleteBrand(int brandId)
        {
            var brand = await GetBrandAsync(brandId);
            if (brand != null)
            {
                Brand.Remove(brand);
                await SaveChangesAsync();
            }
        }

        public async Task DeleteProductType(int id)
        {
            var productType = await GetProductTypeAsync(id);
            if (productType != null)
            {
                ProductTypes.Remove(productType);
                await SaveChangesAsync();
            }
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


        public async Task GenerateMissingLabelsAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).Where(p => p.Label == null).ToListAsync();
            foreach (var product in products)
            {
                await GenerateLabel(basar, product);
            }

            await SaveChangesAsync();
        }

        public async Task<ProductsTransaction> GetAcceptanceAsync(int acceptanceId)
        {
            return await Transactions
                .Include(a => a.Seller)
                    .ThenInclude(s => s.Country)
                .Include(a => a.Products)
                    .ThenInclude(pa => pa.Product)
                        .ThenInclude(p => p.Brand)
                .Include(a => a.Products)
                    .ThenInclude(pa => pa.Product)
                        .ThenInclude(p => p.Type)
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
        
        public async Task<Brand> GetBrandAsync(int brandId)
        {
            return await Brand.FindAsync(brandId);
        }

        public IQueryable<Brand> GetBrands(string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return Brand.OrderBy(b => b.Name);
            }
            return Brand.Where(Expressions.BrandSearch(searchString)).OrderBy(b => b.Name);
        }

        public async Task<FileStore> GetFileAsync(int fileId)
        {
            return await FileStore.FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await Product
                .Include(p => p.Brand)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public IQueryable<Product> GetProducts(string searchString = null)
        {
            var res = Product
                .Include(p => p.Brand).Include(p => p.Type)
                .OrderBy(p => p.Id);

            if (string.IsNullOrEmpty(searchString))
            {
                return res;
            }

            return res.Where(Expressions.ProductSearch(searchString));
        }

        public async Task<IEnumerable<Product>> GetProductsForSaleAsync(int saleId)
        {
            var transaction = await Transactions
                .Include(s => s.Products)
                    .ThenInclude(ps => ps.Product)
                        .ThenInclude(p => p.Brand)
                .Include(a => a.Products)
                    .ThenInclude(pa => pa.Product)
                        .ThenInclude(p => p.Type)
                .FirstOrDefaultAsync(s => s.Id == saleId);
            
            return transaction.Products.Select(t => t.Product);
        }

        public IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId)
        {
            return GetAcceptancesForSeller(basar, sellerId).SelectMany(a => a.Products).Select(pa => pa.Product).Include(p => p.Brand).Include(p => p.Type);
        }

        public async Task<ProductType> GetProductTypeAsync(int id)
        {
            return await ProductTypes.FindAsync(id);
        }
        public IQueryable<ProductType> GetProductTypes(string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return ProductTypes.OrderBy(b => b.Name);
            }
            return ProductTypes.Where(Expressions.ProductTypeSearch(searchString)).OrderBy(b => b.Name);
        }

        public async Task<ProductsTransaction> GetSaleAsync(int saleId)
        {
            return await Transactions
                .Include(s => s.Products)
                    .ThenInclude(ps => ps.Product)
                        .ThenInclude(p => p.Brand)
                .Include(a => a.Products)
                    .ThenInclude(pa => pa.Product)
                        .ThenInclude(p => p.Type)
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
        public async Task<int> GetTransactionNumberForProductAsync(Basar basar, TransactionType type, int productId)
        {
            var transaction = await Transactions.AsNoTracking().Include(t => t.Products).Where(t => t.Type == type && t.BasarId == basar.Id).FirstOrDefaultAsync(t => t.Products.Any(pt => pt.ProductId == productId));
            return transaction.Number;
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

        public async Task<IList<Product>> InsertProductsAsync(IList<Product> products)
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
                    StorageStatus = StorageStatus.Available,
                    TireSize = product.TireSize,
                    TypeId = product.TypeId,
                    ValueStatus = ValueStatus.NotSettled
                };
                newProducts.Add(newProduct);
            }

            await Product.AddRangeAsync(newProducts);
            await SaveChangesAsync();

            return newProducts;
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


        public async Task SetStorageStatusAsync(int basarId, int productId, string notes, StorageStatus storageStatus)
        {
            var product = await GetProductAsync(productId);
            product.StorageStatus = storageStatus;
            
            await SaveChangesAsync();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductToTransaction>().HasKey(ap => new { ap.ProductId, ap.TransactionId});
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
        }
    }
}
