using System.IO;
using Microsoft.Extensions.Caching.Memory;

namespace BraunauMobil.VeloBasar.Tests.VeloCacheTests;

public class GetBannerBytesAsync
{
    [Fact]
    public async Task ShouldReturnFileBytesOnCacheMiss()
    {
        //  Arrange
        VeloCache sut = new(new MemoryCache(new MemoryCacheOptions()));
        string filePath = Path.GetTempFileName();
        byte[] expectedBytes = [1, 2, 3, 4];
        try
        {
            await File.WriteAllBytesAsync(filePath, expectedBytes);

            //  Act
            byte[] result = await sut.GetBannerBytesAsync(filePath);

            //  Assert
            result.Should().Equal(expectedBytes);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task ShouldReturnCachedBytesOnSecondCall()
    {
        //  Arrange
        VeloCache sut = new(new MemoryCache(new MemoryCacheOptions()));
        string filePath = Path.GetTempFileName();
        byte[] originalBytes = [1, 2, 3, 4];
        byte[] changedBytes = [5, 6, 7, 8];
        try
        {
            await File.WriteAllBytesAsync(filePath, originalBytes);
            byte[] firstResult = await sut.GetBannerBytesAsync(filePath);
            await File.WriteAllBytesAsync(filePath, changedBytes);

            //  Act
            byte[] secondResult = await sut.GetBannerBytesAsync(filePath);

            //  Assert
            using (new AssertionScope())
            {
                firstResult.Should().Equal(originalBytes);
                secondResult.Should().Equal(originalBytes);
            }
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task ShouldUseSeparateCacheEntriesPerFilePath()
    {
        //  Arrange
        VeloCache sut = new(new MemoryCache(new MemoryCacheOptions()));
        string filePath1 = Path.GetTempFileName();
        string filePath2 = Path.GetTempFileName();
        byte[] bytes1 = [1, 2, 3, 4];
        byte[] bytes2 = [5, 6, 7, 8];
        try
        {
            await File.WriteAllBytesAsync(filePath1, bytes1);
            await File.WriteAllBytesAsync(filePath2, bytes2);

            //  Act
            byte[] result1 = await sut.GetBannerBytesAsync(filePath1);
            byte[] result2 = await sut.GetBannerBytesAsync(filePath2);

            //  Assert
            using (new AssertionScope())
            {
                result1.Should().Equal(bytes1);
                result2.Should().Equal(bytes2);
            }
        }
        finally
        {
            File.Delete(filePath1);
            File.Delete(filePath2);
        }
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionWhenBannerFilePathIsNull()
    {
        //  Arrange
        VeloCache sut = new(new MemoryCache(new MemoryCacheOptions()));

        //  Act
        Func<Task> act = async () => await sut.GetBannerBytesAsync(null!);

        //  Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
