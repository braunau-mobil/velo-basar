using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ProductStatus
    {
        [Display(Name = "Verfügbar")]
        Available,
        [Display(Name = "Verkauft")]
        Sold,
        [Display(Name = "Gelöscht")]
        Deleted,
        [Display(Name = "Gestohlen")]
        Stolen,
        [Display(Name = "Abgeholt")]
        PickedUp,
        [Display(Name = "Abgerechnet")]
        Settled
    }
}
