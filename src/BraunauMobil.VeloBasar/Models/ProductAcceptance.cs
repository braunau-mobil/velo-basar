namespace BraunauMobil.VeloBasar.Models
{
    public class ProductAcceptance
    {
        public int AcceptanceId { get; set; }

        public Acceptance Acceptance { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
