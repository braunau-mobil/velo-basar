using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SimpleTokenProviderTests
{
    public class CreateToken
    {
        [Theory]
        [InlineData(1, "", "", "VB00100")]
        [InlineData(2, "A", "B", "VB4102420")]
        [InlineData(66, "FirstNameTest", "LastNameTest", "VB4669424C61")]
        [InlineData(572852, "BigIdTest", "BigIdTest", "VB42698BDB44269")]
        public void Test(int id, string firstName, string lastName, string expectedToken)
        {
            var seller = new Seller
            {
                BankAccountHolder = "Test Bank",
                City = "Test City",
                Country = new Country
                {
                    Iso3166Alpha3Code = "AUT",
                    Name = "Austria"
                },
                FirstName = firstName,
                IBAN = "Test IBAN",
                Id = id,
                LastName = lastName,
                PhoneNumber = "Test PhoneNumber",
                Street = "Test Street",
                ZIP = "Test ZIP"
            };

            var provider = new SimpleTokenProvider();
            Assert.Equal(expectedToken, provider.CreateToken(seller));
        }
    }
}
