using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AbstractCrudEntityTests;

public class Ctor
{
    private class TestEntity
        : AbstractCrudEntity
    { }

    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        TestEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.CreatedAt.Should().Be(DateTime.MinValue);
            sut.Id.Should().Be(0);
            sut.State.Should().Be(ObjectState.Enabled);
            sut.UpdatedAt.Should().Be(DateTime.MinValue);
        }
    }
}
