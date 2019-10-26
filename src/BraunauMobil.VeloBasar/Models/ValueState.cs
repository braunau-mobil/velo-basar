using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ValueState
    {
        [Display(Name = "Nicht abgerechnet")]
        NotSettled,
        [Display(Name = "Abgerechnet")]
        Settled
    }
}
