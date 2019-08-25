using BraunauMobil.VeloBasar.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Abrechnung")]
    public class Settlement : ProductTransaction<ProductSettlement>
    {
        public override TransactionType Type => TransactionType.Settlement;
    }
}
