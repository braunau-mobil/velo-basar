using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class ZipMap
    {
        public string Zip { get; set; }
        [Display(Name = "ISO-3166 Code")]
        public int CountryId { get; set; }
        public string City { get; set; }

    }
}
