using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Settlement : ProductTransactionBase<ProductSettlement>
    {
        public override TransactionType Type => TransactionType.Settlement;
    }
}
