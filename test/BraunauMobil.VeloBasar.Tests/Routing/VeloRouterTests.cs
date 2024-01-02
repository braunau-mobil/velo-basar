using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class VeloRouterTests
{
    private readonly VeloRouter _sut = new VeloRouter(new LinkGeneratorMock(), A.Fake<IAcceptProductRouter>(), A.Fake<IAcceptSessionRouter>(), A.Fake<IAdminRouter>(), A.Fake<IBasarRouter>(), A.Fake<ICancelRouter>(), A.Fake<ICartRouter>(), A.Fake<ICountryRouter>(), A.Fake<IDevRouter>(), A.Fake<IAcceptanceLabelsRouter>(), A.Fake<IProductRouter>(), A.Fake<IProductTypeRouter>(), A.Fake<ISellerRouter>(), A.Fake<ISetupRouter>(), A.Fake<ITransactionRouter>());

    [Fact]
    public void ToHome()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToHome();

        //  Assert
        actual.Should().Be("//action=Index&controller=Home");
    }

    [Fact]
    public void ToLogin()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToLogin();

        //  Assert
        actual.Should().Be("//action=Login&controller=Security");
    }

    [Fact]
    public void ToLogout()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToLogout();

        //  Assert
        actual.Should().Be("//action=Logout&controller=Security");
    }

    [Fact]
    public void ToSetTheme()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSetTheme(Theme.Brutal);

        //  Assert
        actual.Should().Be("//theme=Brutal&action=SetTheme&controller=Home");
    }
}
