using System.Text;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AdminServiceTests;

public class ExportSellersForNewsletter
    : TestBase
{
    private int _sellerNumber;
    private CountryEntity _country = new CountryEntity { Name = "Austria", Iso3166Alpha3Code = "AUT" };

    [Theory]
    [InlineData(null)]
    [AutoData]
    public async Task NoSellers_ReturnsCsvWithOnlyHeader(DateTime? minPermissionTimestamp)
    {
        //  Arrange

        //  Act
        FileDataEntity result = await Sut.ExportSellersForNewsletterAsCsvAsync(minPermissionTimestamp);

        //  Assert
        AssertCsv(result, @"﻿Id;FirstName;LastName;Country;City;ZIP;Street;EMail;NewsletterPermissionTimesStamp;UpdatedAt;CreatedAt
");
    }

    [Fact]
    public async Task ReturnsOnlySellersWithPermission()
    {
        //  Arrange
        Clock.Now = new DateTime(2002, 12, 31, 23, 59, 59);
        Db.Sellers.Add(CreateSeller(new DateTime(2062, 04, 05, 11, 22, 33), true));
        Db.Sellers.Add(CreateSeller(new DateTime(2064, 04, 05, 11, 22, 33), true));
        Db.Sellers.Add(CreateSeller(new DateTime(2062, 04, 05, 11, 22, 33), false));
        Db.Sellers.Add(CreateSeller(new DateTime(2064, 04, 05, 11, 22, 33), false));
        await Db.SaveChangesAsync();

        //  Act

        //  Act
        FileDataEntity result = await Sut.ExportSellersForNewsletterAsCsvAsync(null);

        //  Assert
        AssertCsv(result, @"﻿Id;FirstName;LastName;Country;City;ZIP;Street;EMail;NewsletterPermissionTimesStamp;UpdatedAt;CreatedAt
1;FirstName0;LastName0;Austria;City0;ZIP0;Street0;seller0@nope.abc;04/05/2062 11:22:33;12/31/2002 23:59:59;12/31/2002 23:59:59
2;FirstName1;LastName1;Austria;City1;ZIP1;Street1;seller1@nope.abc;04/05/2064 11:22:33;12/31/2002 23:59:59;12/31/2002 23:59:59
");
    }

    [Fact]
    public async Task ReturnsOnlySellersWithPermissionTimestampGreaterOrEqualThan()
    {
        //  Arrange
        Clock.Now = new DateTime(2002, 12, 31, 23, 59, 59);
        Db.Sellers.Add(CreateSeller(new DateTime(2062, 04, 05, 11, 22, 33), true));
        Db.Sellers.Add(CreateSeller(new DateTime(2063, 04, 05, 11, 22, 32), true));
        Db.Sellers.Add(CreateSeller(new DateTime(2063, 04, 05, 11, 22, 33), true));
        Db.Sellers.Add(CreateSeller(new DateTime(2064, 04, 05, 11, 22, 33), true));
        Db.Sellers.Add(CreateSeller(new DateTime(2062, 04, 05, 11, 22, 33), false));
        Db.Sellers.Add(CreateSeller(new DateTime(2063, 04, 05, 11, 22, 32), false));
        Db.Sellers.Add(CreateSeller(new DateTime(2063, 04, 05, 11, 22, 33), false));
        Db.Sellers.Add(CreateSeller(new DateTime(2064, 04, 05, 11, 22, 33), false));
        await Db.SaveChangesAsync();

        //  Act
        FileDataEntity result = await Sut.ExportSellersForNewsletterAsCsvAsync(new DateTime(2063, 04, 05, 11, 22, 33));

        //  Assert
        AssertCsv(result, @"﻿Id;FirstName;LastName;Country;City;ZIP;Street;EMail;NewsletterPermissionTimesStamp;UpdatedAt;CreatedAt
3;FirstName2;LastName2;Austria;City2;ZIP2;Street2;seller2@nope.abc;04/05/2063 11:22:33;12/31/2002 23:59:59;12/31/2002 23:59:59
4;FirstName3;LastName3;Austria;City3;ZIP3;Street3;seller3@nope.abc;04/05/2064 11:22:33;12/31/2002 23:59:59;12/31/2002 23:59:59
");
    }

    private SellerEntity CreateSeller(DateTime permissionTimestamp, bool hasPermission)
    {
        int number = _sellerNumber++;
        return new()
        {
            BankAccountHolder = $"BankAccountHolder{number}",
            City = $"City{number}",
            Comment = $"Comment{number}",
            Country = _country,
            EMail = $"seller{number}@nope.abc",
            FirstName = $"FirstName{number}",
            HasNewsletterPermission = hasPermission,
            IBAN = $"IBAN{number}",
            LastName = $"LastName{number}",
            NewsletterPermissionTimesStamp = permissionTimestamp,
            PhoneNumber = $"PhoneNumer{number}",
            State = ObjectState.Enabled,
            Street = $"Street{number}",
            Token = $"Token{number}",
            ValueState = ValueState.NotSettled,
            ZIP = $"ZIP{number}"
        };
    }

    private static void AssertCsv(FileDataEntity fileData, string expectedCsv)
    {
        using (new AssertionScope())
        {
            fileData.ContentType.Should().Be("text/csv");
            fileData.FileName.Should().Be("sellers.csv");
            fileData.Id.Should().Be(0);

            string actualCsv = Encoding.UTF8.GetString(fileData.Data);
            actualCsv.Should().Be(expectedCsv);
        }
    }
}
