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

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ProductCommission { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ProductDiscount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SellerDiscount { get; set; }
    }
}
