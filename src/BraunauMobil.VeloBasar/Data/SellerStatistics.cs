using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Data
{
    public class SellerStatistics
    {
        public int AceptedProductCount { get; set; }

        public int SoldProductCount { get; set; }

        public int NotSoldProductCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal BillAmout { get; set; }
    }
}
