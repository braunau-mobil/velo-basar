using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ObjectState
    {
        [Display(Name = "Aktiviert")]
        Enabled,
        [Display(Name = "Deaktiviert")]
        Disabled
    }
}
