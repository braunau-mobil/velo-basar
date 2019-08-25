using BraunauMobil.VeloBasar.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Annahme")]
    public class Acceptance : ProductTransaction<ProductAcceptance>
    {
        public override TransactionType Type => TransactionType.Acceptance;
    }
}
