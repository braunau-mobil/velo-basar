namespace BraunauMobil.VeloBasar.Tests.VeloBasarAppContextTests;

public class Version
    : TestBase
{
    [Fact]
    public void ShouldReturnValidVersionNumber()
    {
        //  Arrange

        //  Act
        string version = Sut.Version;

        //  Assert
        System.Version.TryParse(version, out System.Version? _).Should().BeTrue();
    }
}
