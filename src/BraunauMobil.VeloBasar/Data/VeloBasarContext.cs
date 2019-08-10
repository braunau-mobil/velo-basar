using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Npgsql;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloBasarContext : IdentityDbContext
    {
        private const string NextNumberSql = "update \"Number\" set \"Value\"=\"Value\" + 1 where \"BasarId\" = @BasarId and \"Type\" = @Type;select \"Value\" from \"Number\"  where \"BasarId\" = @BasarId and \"Type\" = @Type";

        public VeloBasarContext (DbContextOptions<VeloBasarContext> options)
            : base(options)
        {
        }

        public DbSet<Acceptance> Acceptance { get; set; }

        public DbSet<Basar> Basar { get; set; }

        public DbSet<Billing> Billing { get; set; }

        public DbSet<Cancellation> Cancellation { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<Number> Number { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Sale> Sale { get; set; }

        public DbSet<Seller> Seller { get; set; }

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
            await CreateNewNumberAsync(basar.Id, TransactionType.Billing);
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

        public async Task<Acceptance> GetAcceptanceAsync(int acceptanceId)
        {
            return await Acceptance.FirstAsync(a => a.Id == acceptanceId);
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

        public async Task<string> PrintAcceptanceAsync(int acceptanceId)
        {
            return "~/temp/mypdf.pdf";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAcceptance>().HasKey(ap => new { ap.AcceptanceId, ap.ProductId });
            modelBuilder.Entity<BilledAcceptance>().HasKey(ba => new { ba.BillingId, ba.AcceptanceId });
            modelBuilder.Entity<ProductSale>().HasKey(pp => new { pp.SaleId, pp.ProductId });
            modelBuilder.Entity<Number>().HasKey(n => new { n.BasarId, n.Type });
        }
    }
}
