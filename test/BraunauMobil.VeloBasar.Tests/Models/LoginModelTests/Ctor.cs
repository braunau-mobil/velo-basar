namespace BraunauMobil.VeloBasar.Tests.Models.LoginModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        LoginModel sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Email.Should().BeNull();
            sut.Password.Should().BeNull();
            sut.RememberMe.Should().BeFalse();
            sut.ShowErrorMessage.Should().BeFalse();
        }
    }
}
