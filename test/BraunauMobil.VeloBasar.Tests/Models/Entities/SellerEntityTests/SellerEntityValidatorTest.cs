using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class SellerEntityValidatorTest
{
    private readonly SellerEntityValidator _sut = new(X.StringLocalizer);

    private static SellerEntity CreateValidSeller()
        => new ()
        {
            LastName = "Beutlin",
            FirstName = "Frodo",
            Street = "Hobbingen 1",
            City = "Hobbingen",
            ZIP = "12345",
            CountryId = 1,
            EMail = "frodo.beutlin@auenland.me",
            HasNewsletterPermission = true,
            NewsletterPermissionTimesStamp = new DateTime(2063, 04, 05),
            BankAccountHolder = "Frodo Beutlin",
            IBAN = "QA19VYJP132279842951753866995",
            PhoneNumber = "+1234567890",
            Country = new CountryEntity()
            {
                Id = 1,
                Name = "Auenland",
                Iso3166Alpha3Code = "AUL",
            }
        };

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This First Name is longer than the maximum. So the validation fails. A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z")]
    public void InvalidFirstNames(string? firstName)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.FirstName = firstName!;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.FirstName));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This Last Name is longer than the maximum. So the validation fails. A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z")]
    public void InvalidLastNames(string? lastName)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.LastName = lastName!;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.LastName));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This Street is longer than the maximum. So the validation fails. A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z")]
    public void InvalidStreets(string? street)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.Street = street!;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.Street));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This City is longer than the maximum. So the validation fails. A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z")]
    public void InvalidCities(string? city)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.City = city!;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.City));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void InvalidZIPs(string? zip)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.ZIP = zip!;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.ZIP));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This PhoneNumber is longer than the maximum. So the validation fails. A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z")]
    public void InvalidPhoneNumbers(string? phoneNumber)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.PhoneNumber = phoneNumber!;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.PhoneNumber));
        }
    }

    [Fact]
    public void CountryIdIsNotSet_SouldBeInvalid()
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.CountryId = 0;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.CountryId));
        }
    }

    [Theory]
    [InlineData("This is not a valid IBAN")]
    public void InvalidIBANs(string? iban)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.IBAN = iban;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.IBAN));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void EmptyIBAN_ShouldBeValid(string? iban)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.IBAN = iban;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456")]
    public void InvalidEMails(string? email)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.EMail = email;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(seller.EMail));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ValidEMails_IfPermissionIsNotSet(string? email)
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();
        seller.EMail = email;
        seller.HasNewsletterPermission = false;

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public void ShouldBeValid()
    {
        //  Arrange
        SellerEntity seller = CreateValidSeller();

        //  Act
        ValidationResult result = _sut.Validate(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
