using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ModelState
    {
        [Display(Name = "Aktiviert")]
        Enabled,
        [Display(Name = "Deaktiviert")]
        Disabled
    }
}
