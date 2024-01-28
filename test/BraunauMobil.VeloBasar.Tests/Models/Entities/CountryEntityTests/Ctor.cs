using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.CountryEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        CountryEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.CreatedAt.Should().Be(DateTime.MinValue);
            sut.Id.Should().Be(0);
            sut.Iso3166Alpha3Code.Should().BeNull();
            sut.Name.Should().BeNull();
            sut.UpdatedAt.Should().Be(DateTime.MinValue);
            sut.State.Should().Be(ObjectState.Enabled);
        }   
    }
}
