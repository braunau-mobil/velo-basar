using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.SellerTests
{
    public class GetSmallAddressText
    {
        [Fact]
        public void AllEmpty()
        {
            var seller = new Seller();
            Assert.Equal(" , ,  ",  seller.GetSmallAddressText());
        }
        [Fact]
        public void AllSet()
        {
            var seller = new Seller
            {
                City = "Hopfenhausen",
                Country = new Country
                {
                    Name = "Gerstania",
                },
                FirstName = "Ione",
                LastName = "Bierfrau",
                Street = "Rebengasse",
                ZIP = "1234"
            };
            Assert.Equal("Ione Bierfrau, Rebengasse, 1234 Hopfenhausen", seller.GetSmallAddressText());
        }
    }
}
