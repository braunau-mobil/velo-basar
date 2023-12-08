using System.Net.Mail;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SetupServiceTests;

public class InitializeDatabaseAsync
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task GenerateNothing_ShouldNotGenerateAnything(MailAddress adminUserEmail)
    {
        //  Arrange
        InitializationConfiguration config = new()
        {
            AdminUserEMail = adminUserEmail.ToString()
        };

        //  Act
        await Sut.InitializeDatabaseAsync(config);

        //  Arrange
        Db.AcceptSessions.Should().BeEmpty();
        Db.Basars.Should().BeEmpty();
        Db.Countries.Should().BeEmpty();
        Db.Files.Should().BeEmpty();
        Db.Numbers.Should().BeEmpty();
        Db.Products.Should().BeEmpty();
        Db.ProductToTransaction.Should().BeEmpty();
        Db.ProductTypes.Should().BeEmpty();
        Db.Sellers.Should().BeEmpty();
        Db.Transactions.Should().BeEmpty();
        Db.ZipCodes.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task GenerateCountries_ShouldGenerateCountries(MailAddress adminUserEmail)
    {
        //  Arrange
        InitializationConfiguration config = new()
        {
            AdminUserEMail = adminUserEmail.ToString(),
            GenerateCountries = true
        };

        //  Act
        await Sut.InitializeDatabaseAsync(config);

        //  Arrange
        Db.AcceptSessions.Should().BeEmpty();
        Db.Basars.Should().BeEmpty();
        Db.Countries.Should().NotBeEmpty();
        Db.Files.Should().BeEmpty();
        Db.Numbers.Should().BeEmpty();
        Db.Products.Should().BeEmpty();
        Db.ProductToTransaction.Should().BeEmpty();
        Db.ProductTypes.Should().BeEmpty();
        Db.Sellers.Should().BeEmpty();
        Db.Transactions.Should().BeEmpty();
        Db.ZipCodes.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task GenerateProductTypes_ShouldGenerateProductTypes(MailAddress adminUserEmail)
    {
        //  Arrange
        InitializationConfiguration config = new()
        {
            AdminUserEMail = adminUserEmail.ToString(),
            GenerateProductTypes = true
        };

        //  Act
        await Sut.InitializeDatabaseAsync(config);

        //  Arrange
        Db.AcceptSessions.Should().BeEmpty();
        Db.Basars.Should().BeEmpty();
        Db.Countries.Should().BeEmpty();
        Db.Files.Should().BeEmpty();
        Db.Numbers.Should().BeEmpty();
        Db.Products.Should().BeEmpty();
        Db.ProductToTransaction.Should().BeEmpty();
        Db.ProductTypes.Should().NotBeEmpty();
        Db.Sellers.Should().BeEmpty();
        Db.Transactions.Should().BeEmpty();
        Db.ZipCodes.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task GenerateZipCodes_ShouldGenerateZipCodesAndCountries(MailAddress adminUserEmail)
    {
        //  Arrange
        InitializationConfiguration config = new()
        {
            AdminUserEMail = adminUserEmail.ToString(),
            GenerateCountries = true,
            GenerateZipCodes = true
        };

        //  Act
        await Sut.InitializeDatabaseAsync(config);

        //  Arrange
        Db.AcceptSessions.Should().BeEmpty();
        Db.Basars.Should().BeEmpty();
        Db.Countries.Should().NotBeEmpty();
        Db.Files.Should().BeEmpty();
        Db.Numbers.Should().BeEmpty();
        Db.Products.Should().BeEmpty();
        Db.ProductToTransaction.Should().BeEmpty();
        Db.ProductTypes.Should().BeEmpty();
        Db.Sellers.Should().BeEmpty();
        Db.Transactions.Should().BeEmpty();
        Db.ZipCodes.Should().NotBeEmpty();
    }
}
