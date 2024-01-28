namespace BraunauMobil.VeloBasar.Tests.Models.InitializationConfigurationTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        InitializationConfiguration sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.AdminUserEMail.Should().BeNull();
            sut.GenerateCountries.Should().BeFalse();
            sut.GenerateProductTypes.Should().BeFalse();
            sut.GenerateZipCodes.Should().BeFalse();
        }
    }
}
