using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using BraunauMobil.VeloBasar.Pdf;

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

        public DbSet<Acceptance> Acceptance { get; set; }

        public DbSet<Basar> Basar { get; set; }

        public DbSet<Cancellation> Cancellation { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<FileStore> FileStore { get; set; }

        public DbSet<Number> Number { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Sale> Sale { get; set; }

        public DbSet<Seller> Seller { get; set; }

        public DbSet<Settlement> Settlement { get; set; }

        public async Task<Sale> AddProductToSaleAsync(int basarId, int? saleId, Product product)
        {
            var sale = await CreateOrGetSaleAsync(basarId, saleId);

            if (sale.Products == null)
            {
                sale.Products = new List<ProductSale>();
            }
            sale.Products.Add(new ProductSale
            {
                Product = product,
                Sale = sale
            });

            product.Status = ProductStatus.Sold;

            await SaveChangesAsync();

            return sale;
        }

        public async Task<Acceptance> AcceptProductsAsync(int basarId, int sellerId, int? acceptanceId, params Product[] products)
        {
            Acceptance acceptance;
            if (acceptanceId == null)
            {
                acceptance = new Acceptance
                {
                    BasarId = basarId,
                    SellerId = sellerId,
                    TimeStamp = DateTime.Now,
                    Products = new List<ProductAcceptance>()
                };
            }
            else
            {
                acceptance = await Acceptance.Include(a => a.Products).FirstAsync(a => a.Id == acceptanceId);
            }

            foreach (var product in products)
            {
                await Product.AddAsync(product);

                var productAcceptance = new ProductAcceptance
                {
                    Acceptance = acceptance,
                    Product = product
                };
                acceptance.Products.Add(productAcceptance);
            }

            if (acceptanceId == null)
            {
                acceptance.Number = NextNumber(basarId, TransactionType.Acceptance);
                await Acceptance.AddAsync(acceptance);
            }

            await SaveChangesAsync();

            return acceptance;
        }

        public async Task<Sale> CreateOrGetSaleAsync(int basarId, int? saleId)
        {
            if (saleId == null)
            {
                var sale = new Sale
                {
                    BasarId = basarId,
                    Number = NextNumber(basarId, TransactionType.Sale),
                    TimeStamp = DateTime.Now,
                    Products = new List<ProductSale>()
                };
                Sale.Add(sale);
                await SaveChangesAsync();
                return sale;
            }

            return await Sale.FirstAsync(s => s.Id == saleId);
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

            await CreateNewNumberAsync(basar.Id, TransactionType.Acceptance);
            await CreateNewNumberAsync(basar.Id, TransactionType.Settlement);
            await CreateNewNumberAsync(basar.Id, TransactionType.Cancellation);
            await CreateNewNumberAsync(basar.Id, TransactionType.Sale);

            return basar;
        }

        public async Task CreateNewNumberAsync(int basarId, TransactionType type)
        {
            var number = new Number
            {
                BasarId = basarId,
                Value = 0,
                Type = type
            };
            await Number.AddAsync(number);
            await SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            //  @todo
            return true;
        }

        public async Task<bool> DeleteSellerAsync(int id)
        {
            //  @todo
            return true;
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

        public async Task<FileStore> GenerateAcceptanceDocIfNotExistAsync(int acceptanceId)
        {
            var acceptance = await GetAcceptanceAsync(acceptanceId);
            var fileStore = new FileStore
            {
                ContentType = PdfContentType,
                Data = _pdfCreator.CreateAcceptance(acceptance)
            };
            await FileStore.AddAsync(fileStore);
            await SaveChangesAsync();

            return fileStore;
        }

        public async Task GenerateMissingLabelsAsync(int basarId, int sellerId)
        {
            var basar = await GetBasarAsync(basarId);
            var products = await GetProductsForSeller(basarId, sellerId).Where(p => p.Label == null).ToListAsync();
            foreach (var product in products)
            {
                await GenerateLabel(basar, product);
            }

            await SaveChangesAsync();
        }

        public async Task<FileStore> GenerateSettlementDocIfNotExistAsync(Settlement settlement)
        {
            var fileStore = new FileStore
            {
                ContentType = PdfContentType,
                Data = _pdfCreator.CreateSettlement(settlement)
            };
            await FileStore.AddAsync(fileStore);
            await SaveChangesAsync();

            return fileStore;
        }

        public async Task<Acceptance> GetAcceptanceAsync(int acceptanceId)
        {
            return await Acceptance.FirstAsync(a => a.Id == acceptanceId);
        }

        public IQueryable<Acceptance> GetAcceptancesForSeller(int basarId, int sellerId)
        {
            return Acceptance.Where(a => a.BasarId == basarId && a.SellerId == sellerId);
        }

        public async Task<TransactionStatistic<Acceptance>[]> GetAcceptanceStatisticsAsync(int basarId, int sellerId)
        {
            return await GetAcceptancesForSeller(basarId, sellerId).Include(a => a.Products).AsNoTracking().Select(a => new TransactionStatistic<Acceptance>
            {
                Transaction = a,
                ProductCount = a.Products.Count,
                Amount = a.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }

        public async Task<FileStore> GetAllLabelsAsyncAsCombinedPdf(int basarId, int sellerId)
        {
            var products = await GetProductsForSeller(basarId, sellerId).Where(p => p.Label != null).ToListAsync();
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

        public async Task<FileStore> GetFileAsync(int fileId)
        {
            return await FileStore.FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public IQueryable<Product> GetProductsForSeller(int basarId, int sellerId)
        {
            return GetAcceptancesForSeller(basarId, sellerId).SelectMany(a => a.Products).Select(pa => pa.Product);
        }

        public async Task<SellerStatistics> GetSellerStatisticsAsync(int basarId, int sellerId)
        {
            var products = await GetProductsForSeller(basarId, sellerId).ToArrayAsync();
            var soldProducts = products.State(ProductStatus.Sold).ToArray();
            return new SellerStatistics
            {
                AceptedProductCount = products.Length,
                SettlementAmout = soldProducts.Sum(p => p.Price),
                NotSoldProductCount = products.NotState(ProductStatus.Sold).Count(),
                PickedUpProductCount = products.State(ProductStatus.PickedUp).Count(),
                SoldProductCount = soldProducts.Length
            };
        }

        public IQueryable<Settlement> GetSettlementsForSeller(int basarId, int sellerId)
        {
            return Settlement.Where(s => s.BasarId == basarId && s.SellerId == sellerId);
        }

        public async Task<TransactionStatistic<Settlement>[]> GetSettlementStatisticsAsync(int basarId, int sellerId)
        {
            return await GetSettlementsForSeller(basarId, sellerId).Include(s => s.Products).AsNoTracking().Select(s => new TransactionStatistic<Settlement>
            {
                Transaction = s,
                ProductCount = s.Products.Count,
                Amount = s.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
        }

        public int NextNumber(int basarId, TransactionType transactionType)
        {
            if (basarId <= 0)
            {
                throw new ArgumentException("Invalid basarId");
            }

            var number = -1;
            
            //  @todo i was ned wieso, aber wann i de drecks Connection in using pack, dann krachts da gewaltig
            var connection = Database.GetDbConnection() as NpgsqlConnection;
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = NextNumberSql;
                command.Parameters.AddWithValue("@BasarId", basarId);
                command.Parameters.AddWithValue("@Type", (int)transactionType);

                number = (int) command.ExecuteScalar();
            }

            connection.Close();

            return number;
        }

        public async Task<Settlement> SettleSellerAsync(int basarId, int sellerId)
        {
            var settlement = new Settlement
            {
                BasarId = basarId,
                SellerId = sellerId,
                TimeStamp = DateTime.Now,
                Products = new List<ProductSettlement>()
            };

            var products = await GetProductsForSeller(basarId, sellerId).ToArrayAsync();
            foreach (var product in products)
            {
                if (product.Status == ProductStatus.Available)
                {
                    product.Status = ProductStatus.PickedUp;
                }
                else if (product.Status == ProductStatus.Sold)
                {
                    product.Status = ProductStatus.Settled;
                }

                var productSettlement = new ProductSettlement
                {
                    Product = product,
                    Settlement = settlement
                };
                settlement.Products.Add(productSettlement);
            }

            settlement.Number = NextNumber(basarId, TransactionType.Settlement);
            await Settlement.AddAsync(settlement);

            await SaveChangesAsync();

            return settlement;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAcceptance>().HasKey(ap => new { ap.AcceptanceId, ap.ProductId });
            modelBuilder.Entity<ProductSale>().HasKey(pp => new { pp.SaleId, pp.ProductId });
            modelBuilder.Entity<ProductSettlement>().HasKey(ps => new { ps.SettlementId, ps.ProductId});
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
        }
    }
}
