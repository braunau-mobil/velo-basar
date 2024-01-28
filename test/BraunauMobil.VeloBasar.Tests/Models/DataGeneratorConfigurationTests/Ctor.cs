namespace BraunauMobil.VeloBasar.Tests.Models.DataGeneratorConfigurationTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        // Arrange

        // Act
        DataGeneratorConfiguration sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.AdminUserEMail.Should().BeNull();
            sut.BasarCount.Should().Be(1);
            sut.FirstBasarDate.Should().Be(new DateTime(2063, 04, 05));
            sut.GenerateCountries.Should().BeFalse();
            sut.GenerateProductTypes.Should().BeFalse();
            sut.GenerateZipCodes.Should().BeFalse();
            sut.MaxAcceptancesPerSeller.Should().Be(3);
            sut.MaxSellers.Should().Be(30);
            sut.MeanPrice.Should().Be(100);
            sut.MeanProductsPerSeller.Should().Be(1.3);
            sut.MinAcceptancesPerSeller.Should().Be(1);
            sut.MinSellers.Should().Be(10);
            sut.Seed.Should().Be(-1);
            sut.SimulateBasar.Should().BeFalse();
            sut.StdDevPrice.Should().Be(15);
            sut.StdDevProductsPerSeller.Should().Be(4.5);
        }
    }
}
