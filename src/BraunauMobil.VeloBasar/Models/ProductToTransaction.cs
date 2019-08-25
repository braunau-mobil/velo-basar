namespace BraunauMobil.VeloBasar.Models
{
    public class ProductToTransaction
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int TransactionId { get; set; }

        public ProductsTransaction Transaction { get; set; }
    }
}
