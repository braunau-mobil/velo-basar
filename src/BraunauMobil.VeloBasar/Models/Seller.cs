using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Verkäufer")]
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte einen Vornamen eingeben.")]
        [Display(Name = "Vorname")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bitte einen Nachnamen eingeben.")]
        [Display(Name = "Nachname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Bitte eine Straße eingeben.")]
        [Display(Name = "Straße")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Bitte eine Stadt eingeben.")]
        [Display(Name = "Stadt")]
        public string City { get; set; }

        [Required(ErrorMessage = "Bitte eine Postleitzahl eingeben.")]
        [Display(Name = "PLZ")]
        public string ZIP { get; set; }

        [Required(ErrorMessage = "Bitte ein Land auswählen eingeben.")]
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
    }
}
