using BraunauMobil.VeloBasar.BusinessLogic;
using System.Collections.Generic;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class GetActiveBasarIdAsync
    : SqliteTestBase
{
    [Fact]
    public async Task NoBasars_ReturnsNull()
    {
        //  Arrange
        BasarService sut = new (Db, new OnlyBlackColorProvider());

        //  Act
        int? id = await sut.GetActiveBasarIdAsync();

        //  Assert
        id.Should().BeNull();
    }

    [Fact]
    public async Task NoEnabledBasar_ReturnsNull()
    {
        //  Arrange
        Fixture fixture = new ();
        IEnumerable<BasarEntity> basars = fixture.Build<BasarEntity>()
            .With(basar => basar.State, ObjectState.Disabled)
            .CreateMany();
        Db.Basars.AddRange(basars);
        await Db.SaveChangesAsync();
        BasarService sut = new(Db, new OnlyBlackColorProvider());

        //  Act
        int? id = await sut.GetActiveBasarIdAsync();

        //  Assert
        id.Should().BeNull();
    }

    [Fact]
    public async Task EnabledBasar_ReturnsId()
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<BasarEntity> basars = fixture.Build<BasarEntity>()
            .With(basar => basar.State, ObjectState.Disabled)
            .CreateMany();
        Db.Basars.AddRange(basars);
        BasarEntity enabledBasar = fixture.Build<BasarEntity>()
            .With(basar => basar.State, ObjectState.Enabled)
            .Create();
        Db.Basars.Add(enabledBasar);
        await Db.SaveChangesAsync();
        BasarService sut = new(Db, new OnlyBlackColorProvider());

        //  Act
        int? id = await sut.GetActiveBasarIdAsync();

        //  Assert
        id.Should().Be(enabledBasar.Id);
    }
}
