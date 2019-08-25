using BraunauMobil.VeloBasar.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Storno")]
    public class Cancellation : Transaction
    {
        public int SellerId { get; set; }

        [Display(Name = "Verkäufer")]
        public Seller Seller { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Artikel")]
        public Product Product { get; set; }

        public override TransactionType Type => TransactionType.Cancellation;
    }
}
