using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public sealed class AcceptProductRouterTests
{
    private readonly AcceptProductRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToCreate()
    {
        // Act
        string actual = _sut.ToCreate(123);

        // Assert
        actual.Should().Be("//sessionId=123&action=Create&controller=AcceptProduct");
    }

    [Fact]
    public void ToDelete()
    {
        // Act
        string actual = _sut.ToDelete(987, 666);

        // Assert
        actual.Should().Be("//sessionId=987&productId=666&action=Delete&controller=AcceptProduct");
    }

    [Fact]
    public void ToEdit()
    {
        // Arrange

        // Act
        string actual = _sut.ToEdit(765);

        // Assert
        actual.Should().Be("//productId=765&action=Edit&controller=AcceptProduct");
    }
}