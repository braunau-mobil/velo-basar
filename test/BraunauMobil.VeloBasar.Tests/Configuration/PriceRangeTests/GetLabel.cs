using BraunauMobil.VeloBasar.Configuration;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Tests.Configuration.PriceRangeTests;

public class GetLabel
{
    public static TheoryData<decimal?, decimal?, string> Data => new()
    {
        { null, null, "" },
        { 50M, null, "¤50.00+" },
        { null, 100M, "-¤100.00"},
        { 40M, 80M, "¤40.00 - ¤80.00"},
    };

    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldReturn(decimal? from, decimal? to, string expectedResult)
    {
        //  Arrange
        PriceRange sut = new(from, to);

        //  Act
        string result = sut.GetLabel(CultureInfo.InvariantCulture);

        //  Assert
        result.Should().Be(expectedResult);
    }
}
