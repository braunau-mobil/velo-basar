using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum StorageState
    {
        [Display(Name = "Verfügbar")]
        Available = 0,
        [Display(Name = "Verkauft")]
        Sold = 10,
        [Display(Name = "Verschwunden")]
        Gone = 20,
        [Display(Name = "Gesperrt")]
        Locked = 30
    }
}
