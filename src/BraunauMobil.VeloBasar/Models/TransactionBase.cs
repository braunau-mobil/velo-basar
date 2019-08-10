using System;

namespace BraunauMobil.VeloBasar.Models
{
    public enum TransactionType
    {
        Acceptance,
        Billing,
        Cancellation,
        Sale
    };

    public abstract class TransactionBase : BasarModel
    {
        public int Number { get; set; }

        public DateTime TimeStamp { get; set; }

        public abstract TransactionType Type { get; }
    }
}
