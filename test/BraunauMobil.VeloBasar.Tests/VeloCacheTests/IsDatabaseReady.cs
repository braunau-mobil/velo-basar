using Microsoft.Extensions.Caching.Memory;

namespace BraunauMobil.VeloBasar.Tests.VeloCacheTests;

public class IsDatabaseReady
{
    [Fact]
    public void ShouldBeFalseByDefault()
    {
        //  Arrange
        VeloCache sut = new(new MemoryCache(new MemoryCacheOptions()));

        //  Act
        bool result = sut.IsDatabaseReady;

        //  Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnLastSetValue()
    {
        //  Arrange
        VeloCache sut = new(new MemoryCache(new MemoryCacheOptions()));

        //  Act
        sut.IsDatabaseReady = true;
        bool afterSetTrue = sut.IsDatabaseReady;
        sut.IsDatabaseReady = false;
        bool afterReset = sut.IsDatabaseReady;

        //  Assert
        using (new AssertionScope())
        {
            afterSetTrue.Should().BeTrue();
            afterReset.Should().BeFalse();
        }
    }
}
