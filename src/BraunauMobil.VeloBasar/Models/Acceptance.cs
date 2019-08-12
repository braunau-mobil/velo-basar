using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Acceptance : ProductTransactionBase<ProductAcceptance>
    {
        public override TransactionType Type => TransactionType.Acceptance;
    }
}
