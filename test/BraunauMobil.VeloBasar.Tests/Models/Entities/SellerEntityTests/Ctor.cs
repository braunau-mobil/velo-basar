using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        SellerEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.BankAccountHolder.Should().BeNull();
            sut.City.Should().BeNull();
            sut.Comment.Should().BeNull();
            sut.Country.Should().BeNull();
            sut.CountryId.Should().Be(0);
            sut.CreatedAt.Should().Be(DateTime.MinValue);
            sut.EMail.Should().BeNull();
            sut.FirstName.Should().BeNull();
            sut.HasNewsletterPermission.Should().BeFalse();
            sut.IBAN.Should().BeNull();
            sut.Id.Should().Be(0);
            sut.LastName.Should().BeNull();
            sut.NewsletterPermissionTimesStamp.Should().BeNull();
            sut.PhoneNumber.Should().BeNull();
            sut.State.Should().Be(ObjectState.Enabled);
            sut.Street.Should().BeNull();
            sut.Token.Should().NotBeNull();
            sut.UpdatedAt.Should().Be(DateTime.MinValue);
            sut.ValueState.Should().Be(ValueState.NotSettled);
            sut.ZIP.Should().BeNull();
        }
    }
}
