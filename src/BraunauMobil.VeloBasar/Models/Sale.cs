using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Sale : TransactionBase
    {
        public int Id { get; set; }

        public ICollection<ProductSale> Products { get; set; }

        public override TransactionType Type => TransactionType.Sale;
    }
}
