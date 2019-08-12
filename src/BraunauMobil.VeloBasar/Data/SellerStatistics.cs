using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Data
{
    public class SellerStatistics
    {
        public int AceptedProductCount { get; set; }

        public int SoldProductCount { get; set; }

        public int NotSoldProductCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SettlementAmout { get; set; }
    }
}
