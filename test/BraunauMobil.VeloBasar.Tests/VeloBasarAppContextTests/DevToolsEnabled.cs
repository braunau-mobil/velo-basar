namespace BraunauMobil.VeloBasar.Tests.VeloBasarAppContextTests;

public class DevToolsEnabled
    : TestBase
{
    [Fact]
    public void AreEnabled()
    {
        // Arrange
        A.CallTo(() => WebHostEnvironment.EnvironmentName).Returns("Development");

        // Act
        bool result = Sut.DevToolsEnabled();

        // Assert
        result.Should().BeTrue();

        A.CallTo(() => WebHostEnvironment.EnvironmentName).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    [InlineData("Staging")]
    [InlineData("Production")]
    public void AreNotEnabled(string environmentName)
    {
        // Arrange
        A.CallTo(() => WebHostEnvironment.EnvironmentName).Returns(environmentName);

        // Act
        bool result = Sut.DevToolsEnabled();

        // Assert
        result.Should().BeFalse();

        A.CallTo(() => WebHostEnvironment.EnvironmentName).MustHaveHappenedOnceExactly();
    }
}
