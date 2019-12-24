using BraunauMobil.VeloBasar.Models;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.SellerTests
{
    public class GetBigAddressText
    {
        [Fact]
        public void AllEmpty()
        {
            var seller = new Seller();
            Assert.Equal($" {Environment.NewLine}{Environment.NewLine} {Environment.NewLine}",  seller.GetBigAddressText());
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
            Assert.Equal($"Ione Bierfrau{Environment.NewLine}Rebengasse{Environment.NewLine}1234 Hopfenhausen{Environment.NewLine}", seller.GetBigAddressText());
        }
    }
}
