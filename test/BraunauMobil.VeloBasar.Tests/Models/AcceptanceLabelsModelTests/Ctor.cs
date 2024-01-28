namespace BraunauMobil.VeloBasar.Tests.Models.AcceptanceLabelsModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        AcceptanceLabelsModel model = new();

        //  Assert
        using (new AssertionScope())
        {
            model.Id.Should().Be(0);
            model.Number.Should().Be(0);
            model.OpenDocument.Should().BeFalse();
        }
    }
}
