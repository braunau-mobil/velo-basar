using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.SellerTests
{
    public class GetIdText
    {
        [Fact]
        public void Emtpy()
        {
            var seller = new Seller();
            Assert.Equal("Verk.-ID: 0", seller.GetIdText(TestUtils.CreateLocalizer()));
        }
        [Fact]
        public void NotEmtpy()
        {
            var seller = new Seller
            {
                Id = 666
            };
            Assert.Equal("Verk.-ID: 666", seller.GetIdText(TestUtils.CreateLocalizer()));
        }
    }
}
