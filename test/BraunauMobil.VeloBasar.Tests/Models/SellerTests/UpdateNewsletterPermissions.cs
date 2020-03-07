using BraunauMobil.VeloBasar.Models;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.SellerTests
{
    public class UpdateNewsletterPermissions
    {
        [Fact]
        public void HasPermission()
        {
            var timestamp = new DateTime(2063, 04, 05);
            var seller = new Seller
            {
                HasNewsletterPermission = true,
                NewsletterPermissionTimesStamp = timestamp,
                EMail = "dev@xaka.eu"
            };

            seller.UpdateNewsletterPermissions();

            Assert.True(seller.HasNewsletterPermission);
            DateAssert.IsDateNow(seller.NewsletterPermissionTimesStamp);
            Assert.Equal("dev@xaka.eu", seller.EMail);
        }
        [Fact]
        public void HasNoPermission()
        {
            var timestamp = new DateTime(2063, 04, 05);
            var seller = new Seller
            {
                HasNewsletterPermission = false,
                NewsletterPermissionTimesStamp = timestamp,
                EMail = "dev@xaka.eu"
            };

            seller.UpdateNewsletterPermissions();

            Assert.False(seller.HasNewsletterPermission);
            Assert.Equal(timestamp, seller.NewsletterPermissionTimesStamp);
        }
    }
}
