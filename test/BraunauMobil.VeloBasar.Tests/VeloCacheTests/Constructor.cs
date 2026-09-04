namespace BraunauMobil.VeloBasar.Tests.VeloCacheTests;

public class Constructor
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenMemoryCacheIsNull()
    {
        //  Arrange

        //  Act
        Action act = () => _ = new VeloCache(null!);

        //  Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
