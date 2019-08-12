using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class Basar
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsLocked { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal ProductCommission { get; set; }

        [DataType(DataType.Currency)]
        public decimal ProductDiscount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SellerDiscount { get; set; }
    }
}
