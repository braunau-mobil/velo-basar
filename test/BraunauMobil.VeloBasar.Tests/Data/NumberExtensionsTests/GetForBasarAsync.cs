using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.NumberExtensionsTests;

public class GetForBasarAsync
    : DbTestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnNumbersForBasar(BasarEntity basar, BasarEntity otherBasar)
    {
        // Arrange
        Db.Basars.Add(basar);
        Db.Basars.Add(otherBasar);
        await Db.SaveChangesAsync();
        Db.Numbers.AddRange(
            new NumberEntity { BasarId = basar.Id, Type = TransactionType.Acceptance },
            new NumberEntity { BasarId = otherBasar.Id, Type = TransactionType.Acceptance },
            new NumberEntity { BasarId = basar.Id, Type = TransactionType.Cancellation }
        );
        await Db.SaveChangesAsync();

        // Act
        IReadOnlyList<NumberEntity> result = await Db.Numbers.GetForBasarAsync(basar.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(n => n.BasarId == basar.Id);
    }
}
