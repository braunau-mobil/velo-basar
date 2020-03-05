using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Verkäufer")]
    public class Seller : IModel
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

        [Display(Name = "E-Mail")]
        [EmailAddress(ErrorMessage = "Bitte eine gültige E-Mail Adresse eingeben.")]
        public string EMail { get; set; }

        [Display(Name = "Newsletter")]
        public bool HasNewsletterPermission { get; set; }

        [Display(Name = "Newsletter-Erlaubnis erteilt")]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NewsletterPermissionTimesStamp { get; set; }

        [Required]
        [Display(Name = "Telefonnummer")]
        [Phone(ErrorMessage = "Bitte eine gültige Telefonnummer eingeben")]
        public string PhoneNumber { get; set; }

        [Display(Name = "IBAN")]
        [Iban]
        public string IBAN { get; set; }

        [Display(Name = "Kontoinhaber")]
        public string BankAccountHolder { get; set; }

        public string Token { get; set; }

        [Display(Name = "Abrechnungsstatus")]
        public ValueState ValueState { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        public string GetBigAddressText()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{FirstName} {LastName}");
            sb.AppendLine(Street);
            sb.AppendLine($"{ZIP} {City}");
            return sb.ToString();
        }
        public string GetSmallAddressText()
        {
            return $"{FirstName} {LastName}, {Street}, {ZIP} {City}";
        }
        public string GetIdText(IStringLocalizer<SharedResource> localizer)
        {
            Contract.Requires(localizer != null);

            return localizer["Verk.-ID: {0}", Id];
        }

        public void UpdateNewsletterPermissions()
        {
            if (HasNewsletterPermission)
            {
                NewsletterPermissionTimesStamp = DateTime.Now;
            }
            else
            {
                EMail = null;
            }
        }
    }
}
