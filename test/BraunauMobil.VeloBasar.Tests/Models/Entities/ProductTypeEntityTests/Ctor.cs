using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductTypeEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        ProductTypeEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.CreatedAt.Should().Be(DateTime.MinValue);
            sut.Description.Should().BeNull();
            sut.Id.Should().Be(0);
            sut.Name.Should().BeNull();
            sut.UpdatedAt.Should().Be(DateTime.MinValue);
            sut.State.Should().Be(ObjectState.Enabled);
        }   
    }
}
