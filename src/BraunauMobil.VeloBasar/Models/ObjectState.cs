using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ObjectState
    {
        [Display(Name = "Aktiviert")]
        Enabled = 0,
        [Display(Name = "Deaktiviert")]
        Disabled = 10
    }
}
