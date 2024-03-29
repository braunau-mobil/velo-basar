﻿using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.NumberServiceTests;

public class NextNumberAsync
    : DbTestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ReturnsGreaterNumber(BasarEntity basar, TransactionType transactionType)
    {
        //  Arrange
        NumberService sut = new(Db);
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();
        Db.Numbers.Add(new NumberEntity { Basar = basar, Type = transactionType, Value = 0 });
        await Db.SaveChangesAsync();

        //  Act
        int number1 = await sut.NextNumberAsync(basar.Id, transactionType);
        int number2 = await sut.NextNumberAsync(basar.Id, transactionType);
        int number3 = await sut.NextNumberAsync(basar.Id, transactionType);

        //  Assert
        using (new AssertionScope())
        {
            number1.Should().Be(1);
            number2.Should().Be(number1 + 1);
            number3.Should().Be(number2 + 1);
        }
    }
}
