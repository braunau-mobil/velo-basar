using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models.Base
{
    public class BasarModel
    {
        public int BasarId { get; set; }

        [Display(Name = "Basar")]
        public Basar Basar { get; set; }
    }
}
