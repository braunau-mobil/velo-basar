using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum TransactionType
    {
        [Display(Name = "Annahme")]
        Acceptance,
        [Display(Name = "Storno")]
        Cancellation,
        [Display(Name = "Verkauf")]
        Sale,
        [Display(Name = "Abrechnung")]
        Settlement,
        [Display(Name = "Sperren")]
        Lock,
        [Display(Name = "Als verschwunden markieren")]
        MarkAsGone,
        [Display(Name = "Freischalten")]
        Release
    };
}
