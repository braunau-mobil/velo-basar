﻿using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class InitializationConfiguration
    {
        [Required]
        [Display(Name = "Admin E-Mail")]
        public string AdminUserEMail { get; set; }
        [Display(Name = "Beispiel Länder generieren")]
        public bool GenerateCountries { get; set; }
        [Display(Name = "Beispiel Marken generieren")]
        public bool GenerateBrands { get; set; }
        [Display(Name = "Beispiel Produkt Typen generieren")]
        public bool GenerateProductTypes { get; set; }
        [Display(Name = "Postleitzahlen laden")]
        public bool GenerateZipMap { get; set; }
    }
}
