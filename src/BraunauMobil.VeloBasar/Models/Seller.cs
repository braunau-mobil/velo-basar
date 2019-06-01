namespace BraunauMobil.VeloBasar.Models
{
    public class Seller
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string ZIP { get; set; }

        /// <summary>
        /// https://de.wikipedia.org/wiki/ISO-3166-1-Kodierliste
        /// </summary>
        public string CountryCode { get; set; }

        public string IBAN { get; set; }

        public string BIC { get; set; }

        public string BankAccountHolder { get; set; }

        public string Token { get; set; }
    }
}
