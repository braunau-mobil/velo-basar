using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Verkäufer")]
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vorname")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Straße")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Stadt")]
        public string City { get; set; }

        [Required]
        [Display(Name = "PLZ")]
        public string ZIP { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Display(Name = "Land")]
        public Country Country { get; set; }

        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [Display(Name = "BIC")]
        public string BIC { get; set; }

        [Display(Name = "Kontoinhaber")]
        public string BankAccountHolder { get; set; }

        public string Token { get; set; }

        public bool Match(string searchString)
        {
            return FirstName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || LastName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || City.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || Country.Match(searchString)
                || (BankAccountHolder != null && BankAccountHolder.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
                
        }
    }
}
