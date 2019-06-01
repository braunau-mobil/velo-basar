namespace BraunauMobil.VeloBasar.Models
{
    public class PurchasedProduct
    {
        public int PurchaseId { get; set; }

        public Purchase Purchase { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
