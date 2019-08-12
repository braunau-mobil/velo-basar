using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Settlement : TransactionBase
    {
        public int Id { get; set; }

        public ICollection<ProductSettlement> Products { get; set; }

        public override TransactionType Type => TransactionType.Settlement;
    }
}
