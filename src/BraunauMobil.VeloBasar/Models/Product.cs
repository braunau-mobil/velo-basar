namespace BraunauMobil.VeloBasar.Models
{
    public enum ProductStatus
    {
        Available,
        Sold,
        Deleted,
        Stolen,
        PickedUp
    }

    public class Product
    {
        public int ID { get; set; }

        public string FrameNumber { get; set; }

        public string Color { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string TireSize { get; set; }

        public decimal Price { get; set; }

        public ProductStatus Status { get; set; }
    }
}
