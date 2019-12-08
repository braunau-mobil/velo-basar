using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class InitializationConfiguration
    {
        [Required]
        [Display(Name = "Admin E-Mail")]
        public string AdminUserEMail { get; set; }
    }
}
