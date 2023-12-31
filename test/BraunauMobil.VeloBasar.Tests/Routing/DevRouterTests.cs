using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class DevRouterTests
{
    private readonly DevRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToDeleteCookies()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDeleteCookies();

        //  Assert
        actual.Should().Be("//action=DeleteCookies&controller=Dev");
    }

    [Fact]
    public void ToDropDatabase()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDropDatabase();

        //  Assert
        actual.Should().Be("//action=DropDatabase&controller=Dev");
    }

    [Fact]
    public void ToUnlockAllUsers()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToUnlockAllUsers();

        //  Assert
        actual.Should().Be("//action=UnlockAllUsers&controller=Dev");

    }
}
