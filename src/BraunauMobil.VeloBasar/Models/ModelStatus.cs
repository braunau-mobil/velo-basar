using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ModelStatus
    {
        [Display(Name = "Aktiviert")]
        Enabled,
        [Display(Name = "Deaktiviert")]
        Disabled
    }
}
