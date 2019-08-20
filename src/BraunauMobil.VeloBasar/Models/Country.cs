using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Land")]
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "ISO-3166 Code")]
        public string Iso3166Alpha3Code { get; set; }
    }
}
