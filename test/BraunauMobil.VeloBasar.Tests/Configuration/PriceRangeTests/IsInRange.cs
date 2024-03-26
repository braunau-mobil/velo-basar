using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.Tests.Configuration.PriceRangeTests;

public class IsInRange
{
    public static TheoryData<decimal?, decimal?, decimal, bool> Data => new()
    {
        { null, null, 45654.694M, true },
        { null, null, -454.45M, true },
        { 50M, null, 50M, true },
        { 50M, null, 50.00001M, true },
        { 50M, null, 4056785.565M, true },
        { 50M, null, 49.99999999M, false },
        { 50M, null, 0M, false },
        { 50M, null, -1234.677M, false },
        { null, 100M, 100M, true },
        { null, 100M, 99.9999999M, true },
        { null, 100M, 0M, true },
        { null, 100M, -68673.3434M, true },
        { null, 100M, 100.0000001M, false },
        { null, 100M, 12903812.2323M, false },
        { 40M, 80M, 40M, true },
        { 40M, 80M, 40.00001M, true },
        { 40M, 80M, 76.123M, true },
        { 40M, 80M, 79.9999999M, true },
        { 40M, 80M, 80M, true },
        { 40M, 80M, -123.3454M, false },
        { 40M, 80M, 0M, false },
        { 40M, 80M, 39.999999M, false },
        { 40M, 80M, 80.000001M, false },
        { 40M, 80M, 4359834953.34534M, false },
    };

    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldReturn(decimal? from, decimal? to, decimal value, bool expectedResult)
    {
        //  Arrange
        PriceRange sut = new(from, to);

        //  Act
        bool result = sut.IsInRange(value);

        //  Assert
        result.Should().Be(expectedResult);
    }
}
