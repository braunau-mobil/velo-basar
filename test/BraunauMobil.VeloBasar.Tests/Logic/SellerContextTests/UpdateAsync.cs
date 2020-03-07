using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SellerContextTests
{
    public class UpdateAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void NoNewsletterPermissionDoNotChange()
        {
            await RunOnInitializedDb(async () => 
            {
                var timestamp = new DateTime(2063, 04, 05);
                var fixture = new Fixture();

                var country = await CountryContext.CreateAsync(fixture.Create<Country>());

                var sellerToCreate = fixture.CreateSeller(country);
                sellerToCreate.NewsletterPermissionTimesStamp = timestamp;
                sellerToCreate.HasNewsletterPermission = false;
                
                var createdSeller = await SellerContext.CreateAsync(sellerToCreate);

                Assert.NotNull(createdSeller);
                Assert.NotEqual(0, createdSeller.Id);
                Assert.NotNull(createdSeller.Token);
                Assert.Equal(timestamp, createdSeller.NewsletterPermissionTimesStamp);

                await SellerContext.UpdateAsync(createdSeller);

                Assert.False(createdSeller.HasNewsletterPermission);
                Assert.Equal(timestamp, createdSeller.NewsletterPermissionTimesStamp);
            });
        }
        [Fact]
        public async void NoNewsletterPermissionChange()
        {
            await RunOnInitializedDb(async () =>
            {
                var timestamp = new DateTime(2063, 04, 05);
                var fixture = new Fixture();

                var country = await CountryContext.CreateAsync(fixture.Create<Country>());

                var sellerToCreate = fixture.CreateSeller(country);
                sellerToCreate.NewsletterPermissionTimesStamp = timestamp;
                sellerToCreate.HasNewsletterPermission = false;

                var createdSeller = await SellerContext.CreateAsync(sellerToCreate);

                Assert.NotNull(createdSeller);
                Assert.NotEqual(0, createdSeller.Id);
                Assert.NotNull(createdSeller.Token);
                Assert.Equal(timestamp, createdSeller.NewsletterPermissionTimesStamp);

                createdSeller.HasNewsletterPermission = true;
                createdSeller.EMail = "dev@xaka.eu";
                await SellerContext.UpdateAsync(createdSeller);

                Assert.True(createdSeller.HasNewsletterPermission);
                Assert.Equal("dev@xaka.eu", createdSeller.EMail);
                Assert.True(createdSeller.HasNewsletterPermission);
                DateAssert.IsDateNow(createdSeller.NewsletterPermissionTimesStamp);
            });
        }

        [Fact]
        public async void HasNewsletterPermissionDoNotChange()
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
                Assert.True(createdSeller.HasNewsletterPermission);
                DateAssert.IsDateNow(createdSeller.NewsletterPermissionTimesStamp);

                var timestampBeforeUpdate = createdSeller.NewsletterPermissionTimesStamp;
                await SellerContext.UpdateAsync(createdSeller);

                Assert.NotNull(createdSeller.EMail);
                Assert.True(createdSeller.HasNewsletterPermission);
                Assert.Equal(timestampBeforeUpdate, createdSeller.NewsletterPermissionTimesStamp);
            });
        }
        [Fact]
        public async void HasNewsletterPermissionChange()
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
                Assert.True(createdSeller.HasNewsletterPermission);
                DateAssert.IsDateNow(createdSeller.NewsletterPermissionTimesStamp);

                sellerToCreate.HasNewsletterPermission = false;
                await SellerContext.UpdateAsync(createdSeller);

                Assert.False(createdSeller.HasNewsletterPermission);
            });
        }
    }
}
