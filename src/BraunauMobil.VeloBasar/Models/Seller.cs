using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ZIP { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public string IBAN { get; set; }

        public string BIC { get; set; }

        public string BankAccountHolder { get; set; }

        public string Token { get; set; }
    }
}
