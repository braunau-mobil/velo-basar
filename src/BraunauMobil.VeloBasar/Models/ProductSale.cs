namespace BraunauMobil.VeloBasar.Models
{
    public class ProductSale
    {
        public int SaleId { get; set; }

        public Sale Sale { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
