using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class UpdateNewsletterPermissions
{
    [Theory]
    [AutoData]
    public void HasPermission(string email,  DateTime oldTimeStamp, DateTime newTimeStamp)
    {
        SellerEntity seller = new()
        {
            HasNewsletterPermission = true,
            NewsletterPermissionTimesStamp = oldTimeStamp,
            EMail = email
        };

        seller.UpdateNewsletterPermissions(new ClockMock(newTimeStamp));

        seller.NewsletterPermissionTimesStamp.Should().Be(newTimeStamp);
        seller.HasNewsletterPermission.Should().BeTrue();
        seller.EMail.Should().Be(email);
    }

    [Theory]
    [AutoData]
    public void HasNoPermission(string email, DateTime oldTimeStamp, DateTime newTimeStamp)
    {
        SellerEntity seller = new()
        {
            HasNewsletterPermission = false,
            NewsletterPermissionTimesStamp = oldTimeStamp,
            EMail = email
        };

        seller.UpdateNewsletterPermissions(new ClockMock(newTimeStamp));

        seller.HasNewsletterPermission.Should().BeFalse();
        seller.NewsletterPermissionTimesStamp.Should().Be(oldTimeStamp);
        seller.EMail.Should().Be(email);
    }
}
