namespace BraunauMobil.VeloBasar.Models
{
    public class Cancellation : TransactionBase
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public override TransactionType Type => TransactionType.Cancellation;
    }
}
