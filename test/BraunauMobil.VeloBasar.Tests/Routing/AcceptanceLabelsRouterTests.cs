using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class AcceptanceLabelsRouterTests
{
    private readonly AcceptanceLabelsRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToDownload()
    {
        //  Arrange

        //  Act
        string result = _sut.ToDownload(666);

        //  Assert
        result.Should().Be("//id=666&action=Download&controller=AcceptanceLabels");
    }

    [Fact]
    public void ToSelect()
    {
        //  Arrange

        //  Act
        string result = _sut.ToSelect();

        //  Assert
        result.Should().Be("//action=Select&controller=AcceptanceLabels");
    }
}
