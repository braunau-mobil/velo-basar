using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.SellerTests
{
    public class GetBigAddressText
    {
        [Fact]
        public void AllEmpty()
        {
            var seller = new Seller();
            Assert.Equal(" \r\n\r\n \r\n",  seller.GetBigAddressText());
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
            Assert.Equal("Ione Bierfrau\r\nRebengasse\r\n1234 Hopfenhausen\r\n", seller.GetBigAddressText());
        }
    }
}
