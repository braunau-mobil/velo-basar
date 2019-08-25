using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models.Base
{
    public abstract class ProductTransaction<T> : Transaction
    {
        public int SellerId { get; set; }

        [Display(Name = "Verkäufer")]
        public Seller Seller { get; set; }

        [Display(Name = "Artikel")]
        public ICollection<T> Products { get; set; }
    }
}
