using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SellerContextTests
{
    public class CreateAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void TestWithNoNewsletter()
        {
            await RunOnInitializedDb(async () => 
            {
                var fixture = new Fixture();

                var country = await CountryContext.CreateAsync(fixture.Create<Country>());

                var sellerToCreate = fixture.CreateSeller(country);
                sellerToCreate.HasNewsletterPermission = false;
                
                var createdSeller = await SellerContext.CreateAsync(sellerToCreate);

                Assert.NotNull(createdSeller);
                Assert.NotEqual(0, createdSeller.Id);
                Assert.NotNull(createdSeller.Token);
            });
        }

        [Fact]
        public async void TestWithNewsletter()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var country = await CountryContext.CreateAsync(fixture.Create<Country>());

                var sellerToCreate = fixture.CreateSeller(country);
                sellerToCreate.HasNewsletterPermission = true;

                var createdSeller = await SellerContext.CreateAsync(sellerToCreate);

                Assert.NotNull(createdSeller);
                Assert.NotEqual(0, createdSeller.Id);
                Assert.NotNull(createdSeller.Token);
                Assert.NotNull(createdSeller.EMail);
                //  It is possible that this can fail, but not very likley
                Assert.Equal(DateTime.Now.Year, createdSeller.NewsletterPermissionTimesStamp.Year);
                Assert.Equal(DateTime.Now.Month, createdSeller.NewsletterPermissionTimesStamp.Month);
                Assert.Equal(DateTime.Now.Day, createdSeller.NewsletterPermissionTimesStamp.Day);
            });
        }
    }
}
