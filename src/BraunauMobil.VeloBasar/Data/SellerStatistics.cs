using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Data
{
    [Display(Name = "Verkäufer Statistik")]
    public class SellerStatistics
    {
        [Display(Name = "Angenommene Artikel")]
        public int AceptedProductCount { get; set; }

        [Display(Name = "Verkaufte Artikel")]
        public int SoldProductCount { get; set; }

        [Display(Name = "Nicht verkaufte Artikel")]
        public int NotSoldProductCount { get; set; }

        [Display(Name = "Abgeholte Artikel")]
        public int PickedUpProductCount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Abgerechneter Betrag")]
        public decimal SettlementAmout { get; set; }
    }
}
