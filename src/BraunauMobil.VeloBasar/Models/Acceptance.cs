using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Acceptance : TransactionBase
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public ICollection<ProductAcceptance> Products { get; set; }

        public override TransactionType Type => TransactionType.Acceptance;
    }
}
