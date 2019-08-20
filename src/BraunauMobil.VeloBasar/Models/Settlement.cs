using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Abrechnung")]
    public class Settlement : ProductTransactionBase<ProductSettlement>
    {
        public override TransactionType Type => TransactionType.Settlement;
    }
}
