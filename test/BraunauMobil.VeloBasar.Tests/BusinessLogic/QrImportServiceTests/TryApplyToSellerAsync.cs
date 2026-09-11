using BraunauMobil.VeloBasar.BusinessLogic;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.QrImportServiceTests;

public class TryApplyToSellerAsync
    : TestBase
{
    [Theory]
    [VeloInlineAutoData(null!)]
    [VeloInlineAutoData("")]
    [VeloInlineAutoData("   ")]
    public async Task NullOrBlankText_ReturnsEmpty(string? qrText, SellerEntity seller)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(qrText, seller);

        //  Assert
        result.Status.Should().Be(QrImportStatus.Empty);
    }

    [Theory]
    [VeloInlineAutoData("this is not a velo-basar payload")]
    [VeloInlineAutoData("2\tseller\tAnna\tMuster\tHauptplatz 1\tBraunau am Inn\t5280\tAUT\t\t+43 664 1234567\t\t\tfalse")]
    [VeloInlineAutoData("VB3S")]
    [VeloInlineAutoData("VB3S85N6SO899LQN6T35E84KGOBLE1Q70R31EHT2-")]
    public async Task InvalidFormat_ReturnsInvalidFormat(string qrText, SellerEntity seller)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(qrText, seller);

        //  Assert
        result.Status.Should().Be(QrImportStatus.InvalidFormat);
    }

    [Theory]
    [VeloAutoData]
    public async Task TooFewFields_ReturnsInvalidFormat(SellerEntity seller)
    {
        //  Arrange
        string qrText = QrPayload.Encode(3, 'S', "Anna", "Muster", "Hauptplatz 1");

        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(qrText, seller);

        //  Assert
        result.Status.Should().Be(QrImportStatus.InvalidFormat);
    }

    [Theory]
    [VeloAutoData]
    public async Task WrongKind_ReturnsWrongKind(SellerEntity seller)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(QrPayload.Product, seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.WrongKind);
            result.ActualKind.Should().Be("product");
        }
    }

    [Theory]
    [VeloInlineAutoData(2)]
    [VeloInlineAutoData(4)]
    public async Task UnsupportedSchemaVersion_ReturnsUnsupportedSchemaVersion(int schemaVersion, SellerEntity seller)
    {
        //  Arrange
        string qrText = QrPayload.Encode(schemaVersion, 'S', "Anna", "Muster", "Hauptplatz 1", "Braunau am Inn", "5280", "AUT", "", "+43 664 1234567", "", "", "false");

        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(qrText, seller);

        //  Assert
        result.Status.Should().Be(QrImportStatus.UnsupportedSchemaVersion);
    }

    [Theory]
    [VeloAutoData]
    public async Task DroppedCharacter_ReturnsChecksumMismatch(SellerEntity seller)
    {
        //  Arrange
        string qrText = QrPayload.DropCharacter(QrPayload.Seller, 20);

        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(qrText, seller);

        //  Assert
        result.Status.Should().Be(QrImportStatus.ChecksumMismatch);
    }

    [Theory]
    [VeloAutoData]
    public async Task WrongCharacter_ReturnsChecksumMismatch(SellerEntity seller)
    {
        //  Arrange
        string qrText = QrPayload.ReplaceCharacter(QrPayload.Seller, 30);

        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(qrText, seller);

        //  Assert
        result.Status.Should().Be(QrImportStatus.ChecksumMismatch);
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_KnownCountry_MapsFieldsAndReturnsSuccess(SellerEntity seller)
    {
        //  Arrange
        CountryEntity country = Fixture.BuildCountry()
            .With(c => c.Iso3166Alpha3Code, "AUT")
            .With(c => c.State, ObjectState.Enabled)
            .Create();
        Db.Countries.Add(country);
        await Db.SaveChangesAsync();

        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(QrPayload.Seller, seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            result.UnmatchedCountryIso3.Should().BeNull();

            seller.FirstName.Should().Be("Anna");
            seller.LastName.Should().Be("Muster");
            seller.Street.Should().Be("Hauptplatz 1");
            seller.City.Should().Be("Braunau am Inn");
            seller.ZIP.Should().Be("5280");
            seller.CountryId.Should().Be(country.Id);
            seller.EMail.Should().Be("anna.muster@example.com");
            seller.PhoneNumber.Should().Be("+43 664 1234567");
            seller.IBAN.Should().Be("AT611904300234573201");
            seller.BankAccountHolder.Should().BeNull();
            seller.HasNewsletterPermission.Should().BeTrue();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_Lowercase_ReturnsSuccess(SellerEntity seller)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(QrPayload.Seller.ToLowerInvariant(), seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            seller.EMail.Should().Be("anna.muster@example.com");
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_UnknownCountry_LeavesCountryIdZeroAndReturnsUnmatchedCountry(SellerEntity seller)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(QrPayload.Seller, seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            result.UnmatchedCountryIso3.Should().Be("AUT");
            seller.CountryId.Should().Be(0);
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_OmittedOptionalFields_ClearsExistingValues(SellerEntity seller)
    {
        //  Arrange
        seller.EMail = "old@example.com";
        seller.IBAN = "AT000000000000000000";
        seller.BankAccountHolder = "Old Holder";

        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(QrPayload.SellerWithoutOptionalFields, seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            seller.EMail.Should().BeNull();
            seller.IBAN.Should().BeNull();
            seller.BankAccountHolder.Should().BeNull();
            seller.HasNewsletterPermission.Should().BeFalse();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_NonAsciiAndBackslash_DecodesFields(SellerEntity seller)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToSellerAsync(QrPayload.SellerWithNonAscii, seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            seller.FirstName.Should().Be("Jürgen");
            seller.LastName.Should().Be("Müller");
            seller.Street.Should().Be("Straße 1");
            seller.BankAccountHolder.Should().Be("C:\\Temp ß€");
        }
    }
}
