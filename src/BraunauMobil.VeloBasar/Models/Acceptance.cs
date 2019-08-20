using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Annahme")]
    public class Acceptance : ProductTransactionBase<ProductAcceptance>
    {
        public override TransactionType Type => TransactionType.Acceptance;
    }
}
