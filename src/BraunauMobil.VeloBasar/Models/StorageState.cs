using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum StorageState
    {
        [Display(Name = "Verfügbar")]
        Available,
        [Display(Name = "Verkauft")]
        Sold,
        [Display(Name = "Verschwunden")]
        Gone,
        [Display(Name = "Gesperrt")]
        Locked
    }
}
