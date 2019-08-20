using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Verkauf")]
    public class Sale : TransactionBase
    {
        public ICollection<ProductSale> Products { get; set; }

        public override TransactionType Type => TransactionType.Sale;
    }
}
