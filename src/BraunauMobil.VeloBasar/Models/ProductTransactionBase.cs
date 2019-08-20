using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public abstract class ProductTransactionBase<T> : TransactionBase
    {
        public int SellerId { get; set; }

        [Display(Name = "Verkäufer")]
        public Seller Seller { get; set; }

        [Display(Name = "Artikel")]
        public ICollection<T> Products { get; set; }
    }
}
