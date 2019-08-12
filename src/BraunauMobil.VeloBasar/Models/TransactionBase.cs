using System;

namespace BraunauMobil.VeloBasar.Models
{
    public enum TransactionType
    {
        Acceptance,
        Cancellation,
        Sale,
        Settlement,
    };

    public abstract class TransactionBase : BasarModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime TimeStamp { get; set; }

        public abstract TransactionType Type { get; }
    }
}
