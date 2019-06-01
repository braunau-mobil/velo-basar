namespace BraunauMobil.VeloBasar.Models
{
    public class BilledAcceptance
    {
        public int BillingId { get; set; }

        public Billing Billing { get; set; }

        public int AcceptanceId { get; set; }

        public Acceptance Acceptance { get; set; }
    }
}
