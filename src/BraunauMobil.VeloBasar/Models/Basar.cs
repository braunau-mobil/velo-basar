using System;

namespace BraunauMobil.VeloBasar.Models
{
    public class Basar
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public decimal ProductCommission { get; set; }

        public decimal ProductDiscount { get; set; }

        public decimal SellerDiscount { get; set; }
    }
}
