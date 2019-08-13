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

        public async Task<Sale> AddProductToSaleAsync(Basar basar, int? saleId, Product product)
        {
            var sale = await CreateOrGetSaleAsync(basar, saleId);

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

        public async Task<Acceptance> AcceptProductsAsync(Basar basar, int sellerId, int? acceptanceId, params Product[] products)
        {
            Acceptance acceptance;
            if (acceptanceId == null)
            {
                acceptance = new Acceptance
                {
                    Basar = basar,
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
                acceptance.Number = NextNumber(basar, TransactionType.Acceptance);
                await Acceptance.AddAsync(acceptance);
            }

            await SaveChangesAsync();

            return acceptance;
        }

        public async Task<Sale> CreateOrGetSaleAsync(Basar basar, int? saleId)
        {
            if (saleId == null)
            {
                var sale = new Sale
                {
                    Basar = basar,
                    Number = NextNumber(basar, TransactionType.Sale),
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

        public async Task<FileStore> GenerateSettlementDocIfNotExistAsync(Basar basar, Settlement settlement)
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

        public async Task<Acceptance> GetAcceptanceAsync(int acceptanceId)
        {
            return await Acceptance.FirstAsync(a => a.Id == acceptanceId);
        }

        public IQueryable<Acceptance> GetAcceptancesForSeller(Basar basar, int sellerId)
        {
            return Acceptance.Where(a => a.BasarId == basar.Id && a.SellerId == sellerId);
        }

        public async Task<TransactionStatistic<Acceptance>[]> GetAcceptanceStatisticsAsync(Basar basar, int sellerId)
        {
            return await GetAcceptancesForSeller(basar, sellerId).Include(a => a.Products).AsNoTracking().Select(a => new TransactionStatistic<Acceptance>
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

        public async Task<FileStore> GetFileAsync(int fileId)
        {
            return await FileStore.FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId)
        {
            return GetAcceptancesForSeller(basar, sellerId).SelectMany(a => a.Products).Select(pa => pa.Product);
        }

        public async Task<Sale> GetSaleAsync(int saleId)
        {
            return await Sale.FirstOrDefaultAsync(s => s.Id == saleId);
        }

        public async Task<SellerStatistics> GetSellerStatisticsAsync(Basar basar, int sellerId)
        {
            var products = await GetProductsForSeller(basar, sellerId).ToArrayAsync();
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

        public IQueryable<Settlement> GetSettlementsForSeller(Basar basar, int sellerId)
        {
            return Settlement.Where(s => s.BasarId == basar.Id && s.SellerId == sellerId);
        }

        public async Task<TransactionStatistic<Settlement>[]> GetSettlementStatisticsAsync(Basar basar, int sellerId)
        {
            return await GetSettlementsForSeller(basar, sellerId).Include(s => s.Products).AsNoTracking().Select(s => new TransactionStatistic<Settlement>
            {
                Transaction = s,
                ProductCount = s.Products.Count,
                Amount = s.Products.Sum(p => p.Product.Price)
            }).ToArrayAsync();
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

        public async Task<Settlement> SettleSellerAsync(Basar basar, int sellerId)
        {
            var settlement = new Settlement
            {
                Basar = basar,
                SellerId = sellerId,
                TimeStamp = DateTime.Now,
                Products = new List<ProductSettlement>()
            };

            var products = await GetProductsForSeller(basar, sellerId).ToArrayAsync();
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

            settlement.Number = NextNumber(basar, TransactionType.Settlement);
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
