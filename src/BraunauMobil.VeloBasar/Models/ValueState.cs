using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ValueState
    {
        [Display(Name = "Nicht abgerechnet")]
        NotSettled = 0,
        [Display(Name = "Abgerechnet")]
        Settled = 10
    }
}
