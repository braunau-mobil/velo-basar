namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.WordPressStatusPushServiceTests;

public class IsEnabled
    : TestBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ShouldReturnSettingsEnabled(bool enabled)
    {
        //  Arrange
        Settings.Enabled = enabled;

        //  Act
        bool result = Sut.IsEnabled;

        //  Assert
        result.Should().Be(enabled);
    }
}
