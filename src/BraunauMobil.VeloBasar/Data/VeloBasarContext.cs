using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloBasarContext : DbContext
    {
        public VeloBasarContext (DbContextOptions<VeloBasarContext> options)
            : base(options)
        {
        }

        public DbSet<Acceptance> Acceptance { get; set; }

        public DbSet<Basar> Basar { get; set; }

        public DbSet<Billing> Billing { get; set; }

        public DbSet<Cancellation> Cancellation { get; set; }

        //public DbSet<Number> Number { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Purchase> Purchase { get; set; }

        public DbSet<Seller> Seller { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAcceptance>().HasKey(ap => new { ap.AcceptanceId, ap.ProductId });
            modelBuilder.Entity<BilledAcceptance>().HasKey(ba => new { ba.BillingId, ba.AcceptanceId });
            modelBuilder.Entity<PurchasedProduct>().HasKey(pp => new { pp.PurchaseId, pp.ProductId });
        }
    }
}
