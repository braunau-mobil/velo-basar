namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class UpdateNewsletterPermissions
{
    [Theory]
    [VeloAutoData]
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
    [VeloAutoData]
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
