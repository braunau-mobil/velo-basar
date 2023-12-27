namespace BraunauMobil.VeloBasar.Tests.VeloBasarAppContextTests;

public class IsDatabaseInitialized
    : TestBase
{
    [Fact]
    public void IsInitialized()
    {
        //  Arrange

        //  Act
        bool result = Sut.IsDatabaseInitialized();

        //  Assert
        result.Should().BeTrue();
    }
}
