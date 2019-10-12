using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ValueStatus
    {
        [Display(Name = "Nicht abgerechnet")]
        NotSettled,
        [Display(Name = "Abgerechnet")]
        Settled
    }
}
