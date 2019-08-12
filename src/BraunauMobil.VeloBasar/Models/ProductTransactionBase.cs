using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public abstract class ProductTransactionBase<T> : TransactionBase
    {
        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public ICollection<T> Products { get; set; }
    }
}
