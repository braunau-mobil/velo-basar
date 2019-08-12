namespace BraunauMobil.VeloBasar.Models
{
    public class ProductSettlement
    {
        public int SettlementId { get; set; }

        public Settlement Settlement { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
