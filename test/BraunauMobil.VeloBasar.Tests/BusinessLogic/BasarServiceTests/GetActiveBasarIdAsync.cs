using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class GetActiveBasarIdAsync
    : TestBase
{
    [Fact]
    public async Task NoBasars_ReturnsNull()
    {
        //  Arrange

        //  Act
        int? id = await Sut.GetActiveBasarIdAsync();

        //  Assert
        id.Should().BeNull();
    }

    [Fact]
    public async Task NoEnabledBasar_ReturnsNull()
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<BasarEntity> basars = fixture.Build<BasarEntity>()
            .With(_ => _.State, ObjectState.Disabled)
            .CreateMany();
        Db.Basars.AddRange(basars);
        await Db.SaveChangesAsync();

        //  Act
        int? id = await Sut.GetActiveBasarIdAsync();

        //  Assert
        id.Should().BeNull();
    }

    [Fact]
    public async Task EnabledBasar_ReturnsId()
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<BasarEntity> basars = fixture.Build<BasarEntity>()
            .With(_ => _.State, ObjectState.Disabled)
            .CreateMany();
        Db.Basars.AddRange(basars);
        BasarEntity enabledBasar = fixture.Build<BasarEntity>()
            .With(_ => _.State, ObjectState.Enabled)
            .Create();
        Db.Basars.Add(enabledBasar);
        await Db.SaveChangesAsync();

        //  Act
        int? id = await Sut.GetActiveBasarIdAsync();

        //  Assert
        id.Should().Be(enabledBasar.Id);
    }
}
