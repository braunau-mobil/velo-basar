using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Purchase : TransactionBase
    {
        public int Id { get; set; }

        public ICollection<PurchasedProduct> Products { get; set; }

        public override TransactionType Type => TransactionType.Purchase;
    }
}
