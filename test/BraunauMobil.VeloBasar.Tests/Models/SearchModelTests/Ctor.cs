namespace BraunauMobil.VeloBasar.Tests.Models.SearchModelTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void CheckDefaults(string searchString, string resetUrl)
    {
        //  Arrange

        //  Act
        SearchModel model = new(searchString, resetUrl);

        //  Assert
        using (new AssertionScope())
        {
            model.SearchString.Should().Be(searchString);
            model.ResetUrl.Should().Be(resetUrl);
        }
    }
}
