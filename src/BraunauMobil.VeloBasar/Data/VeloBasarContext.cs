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
            //  1. instert all the products
            var productsInserted = await InsertProductsAsync(products);

            var acceptance = new ProductsTransaction
            {
                Type = TransactionType.Acceptance,
                Basar = basar,
                SellerId = sellerId,
                TimeStamp = DateTime.Now,
                Products = new List<ProductToTransaction>()
            };

            acceptance.Products = productsInserted.Select(p => new ProductToTransaction { Product = p, Transaction = acceptance }).ToList();
            Transactions.Add(acceptance);
            await SaveChangesAsync();

            await GenerateAcceptanceDocAsync(basar, acceptance);

            return acceptance;
        }
        public async Task<ProductsTransaction> CancelProductsAsync(Basar basar, int saleId, int[] productIds)
        {
            var sale = await Transactions.GetAsync(saleId);
            var cancellation = new ProductsTransaction
            {
                Type = TransactionType.Cancellation,
                Basar = basar,
                Number = NextNumber(basar, TransactionType.Cancellation),
                TimeStamp = DateTime.Now,
                Products = new List<ProductToTransaction>()
            };
            foreach (var productId in productIds)
            {
                var product = await GetProductAsync(productId);
                cancellation.Products.Add(new ProductToTransaction
                {
                    Transaction = cancellation,
                    Product = product
                });
                product.StorageStatus = StorageStatus.Available;

                sale.Products.Remove(sale.Products.First(s => s.ProductId == product.Id));
            }
            await Transactions.AddAsync(cancellation);

            if (sale.Products.Count <= 0)
            {
                Transactions.Remove(sale);
            }
            else
            {
                await GenerateSaleDocAsync(basar, sale);
            }

            await SaveChangesAsync();

            return cancellation;
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
            //  1. checkout all products
            var products = await Product.Get(productIds).AsTracking().ToArrayAsync();
            if (products.Any(p => !p.CanSell()))
            {
                return null;
            }
            foreach (var product in products)
            {
                product.StorageStatus = StorageStatus.Sold;
            }
            await SaveChangesAsync();

            var sale = new ProductsTransaction
            {
                Type = TransactionType.Sale,
                Basar = basar,
                Number = NextNumber(basar, TransactionType.Sale),
                TimeStamp = DateTime.Now,
            };
            sale.Products = products.Select(p => new ProductToTransaction { Product = p, Transaction = sale }).ToList();
            Transactions.Add(sale);            
            await SaveChangesAsync();

            await GenerateSaleDocAsync(basar, sale);
            
            return sale;
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

        public async Task<FileStore> GenerateAcceptanceDocAsync(Basar basar, ProductsTransaction sale)
        {
            FileStore fileStore;
            if (sale.DocumentId == null)
            {
                fileStore = new FileStore
                {
                    ContentType = PdfContentType
                };
                await FileStore.AddAsync(fileStore);
                await SaveChangesAsync();

                sale.DocumentId = fileStore.Id;
            }
            else
            {
                fileStore = await GetFileAsync(sale.DocumentId.Value);
            }
            fileStore.Data = _pdfCreator.CreateAcceptance(basar, sale);
            await SaveChangesAsync();

            return fileStore;
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

        public async Task<FileStore> GenerateSaleDocAsync(Basar basar, ProductsTransaction sale)
        {
            FileStore fileStore;
            if (sale.DocumentId == null)
            {
                fileStore = new FileStore
                {
                    ContentType = PdfContentType
                };
                await FileStore.AddAsync(fileStore);
                await SaveChangesAsync();

                sale.DocumentId = fileStore.Id;
            }
            else
            {
                fileStore = await GetFileAsync(sale.DocumentId.Value);
            }
            fileStore.Data = _pdfCreator.CreateSale(basar, sale);
            await SaveChangesAsync();

            return fileStore;
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
                .Include(p => p.Brand).Include(p => p.Type)
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
                    Notes = product.Notes,
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
