using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Data
{
    public class InitializationConfiguration
    {
        [Required]
        [Display(Name = "Admin E-Mail")]
        public string AdminUserEMail { get; set; }
    }
}
