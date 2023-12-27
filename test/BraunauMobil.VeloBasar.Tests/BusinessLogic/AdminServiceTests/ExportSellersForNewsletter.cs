using System.Text;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AdminServiceTests;

public class ExportSellersForNewsletter
    : TestBase
{
    [Fact]
    public async Task NoSellers_ReturnsCsvWithOnlyHeader()
    {
        //  Arrange

        //  Act
        FileDataEntity result = await Sut.ExportSellersForNewsletterAsCsvAsync();

        //  Assert
        AssertCsv(result, lines =>
        {
            lines.Should().HaveCount(2);
            lines.Should().Contain("﻿Id;FirstName;LastName;Country;City;ZIP;Street;EMail;NewsletterPermissionTimesStamp;UpdatedAt;CreatedAt");
            lines.Should().Contain("");
        });
    }

    [Fact]
    public async Task ReturnsOnlySellersWithPermission()
    {
        //  Arrange
        Clock.Now = new DateTime(2002, 12, 31, 23, 59, 59);
        CountryEntity country = new ()
        {
            Name = "Austria",
            Iso3166Alpha3Code = "AUT",
        };
        SellerEntity sellerWithPermission = new ()
        {
            BankAccountHolder = "BankAccountHolder1",
            City = "City1",
            Comment = "Comment1",
            Country = country,
            EMail = "seller1@nope.abc",
            FirstName = "FirstName1",
            HasNewsletterPermission = true,
            IBAN = "AT222250053952791316",
            LastName = "LastName1",
            NewsletterPermissionTimesStamp = new DateTime(2063, 04, 05, 11, 22, 33),
            PhoneNumber = "PhoneNumer1",
            State = ObjectState.Enabled,
            Street = "Street1",
            Token = "Token1",
            ValueState = ValueState.NotSettled,
            ZIP = "ZIP1"
        };
        Db.Sellers.Add(sellerWithPermission);
        SellerEntity sellerWithoutPermission = new()
        {
            BankAccountHolder = "BankAccountHolder2",
            City = "City2",
            Comment = "Comment2",
            Country = country,
            EMail = "seller2@nope.abc",
            FirstName = "FirstName2",
            HasNewsletterPermission = false,
            IBAN = "AT222250053952791416",
            LastName = "LastName2",
            NewsletterPermissionTimesStamp = new DateTime(2063, 04, 05, 11, 22, 44),
            PhoneNumber = "PhoneNumer2",
            State = ObjectState.Enabled,
            Street = "Street2",
            Token = "Token2",
            ValueState = ValueState.NotSettled,
            ZIP = "ZIP1"
        };
        Db.Sellers.Add(sellerWithoutPermission);
        await Db.SaveChangesAsync();

        //  Act

        //  Act
        FileDataEntity result = await Sut.ExportSellersForNewsletterAsCsvAsync();

        //  Assert
        AssertCsv(result, lines =>
        {
            lines.Should().HaveCount(3);
            lines.Should().Contain("﻿Id;FirstName;LastName;Country;City;ZIP;Street;EMail;NewsletterPermissionTimesStamp;UpdatedAt;CreatedAt");
            lines.Should().Contain("1;FirstName1;LastName1;Austria;City1;ZIP1;Street1;seller1@nope.abc;04/05/2063 11:22:33;12/31/2002 23:59:59;12/31/2002 23:59:59");
            lines.Should().Contain("");
        });
    }

    private static void AssertCsv(FileDataEntity fileData, Action<string[]> doWithLines)
    {
        using (new AssertionScope())
        {
            fileData.ContentType.Should().Be("text/csv");
            fileData.FileName.Should().Be("sellers.csv");
            fileData.Id.Should().Be(0);

            string text = Encoding.UTF8.GetString(fileData.Data);
            string[] lines = text.Split(Environment.NewLine);
            doWithLines(lines);
        }
    }
}
