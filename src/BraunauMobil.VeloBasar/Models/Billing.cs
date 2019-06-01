using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Models
{
    public class Billing : TransactionBase
    {
        public int Id { get; set; }

        public ICollection<BilledAcceptance> Acceptances { get; set; }

        public override TransactionType Type => TransactionType.Billing;
    }
}
