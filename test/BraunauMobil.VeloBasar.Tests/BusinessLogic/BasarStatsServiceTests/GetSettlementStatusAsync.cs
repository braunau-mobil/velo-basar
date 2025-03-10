﻿using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSettlementStatusAsync
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task SettlementStarted_ShouldBeCorrect(BasarEntity basar, TransactionEntity settlement)
    {
        //  Arrange
        await InsertSellersAndProducts(basar);
        settlement.Type = TransactionType.Settlement;
        settlement.Basar = basar;
        settlement.BasarId = basar.Id;
        settlement.Seller = null;
        Db.Transactions.Add(settlement);
        await Db.SaveChangesAsync();

        //  Act
        BasarSettlementStatus result = await Sut.GetSettlementStatusAsync(basar);

        //  Assert
        using (new AssertionScope())
        {
            result.HasSettlementStarted.Should().BeTrue();

            result.OverallNotSettledCount.Should().Be(18);
            result.RemoteCount.Should().Be(9);
            result.OnSiteCount.Should().Be(9);
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task NotSettlementTransaction_SettlementStatusShouldBeNotStarted(BasarEntity basar)
    {
        //  Arrange
        await InsertSellersAndProducts(basar);

        //  Act
        BasarSettlementStatus result = await Sut.GetSettlementStatusAsync(basar);

        //  Assert
        using (new AssertionScope())
        {
            result.HasSettlementStarted.Should().BeFalse();

            result.OverallNotSettledCount.Should().Be(0);
            result.RemoteCount.Should().Be(0);
            result.OnSiteCount.Should().Be(0);
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task DisabledBasar_SettlementStatusShouldBeNotStarted(BasarEntity basar)
    {
        //  Arrange
        basar.State = ObjectState.Disabled;
        Db.Basars.Add(basar);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.Settled, StorageState.Available, false)]);
        await Db.SaveChangesAsync();

        //  Act
        BasarSettlementStatus result = await Sut.GetSettlementStatusAsync(basar);

        //  Assert
        using (new AssertionScope())
        {
            result.HasSettlementStarted.Should().BeFalse();

            result.OverallNotSettledCount.Should().Be(0);
            result.RemoteCount.Should().Be(0);
            result.OnSiteCount.Should().Be(0);            
        }
    }

    private async Task InsertSellersAndProducts(BasarEntity basar)
    {
        basar.State = ObjectState.Enabled;
        Db.Basars.Add(basar);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Available, false)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Available, true)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Lost, false)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Lost, true)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Locked, false)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Locked, true)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Sold, false)]);
        CreateSellerAndProducts(basar, null, ValueState.Settled, [(ValueState.NotSettled, StorageState.Sold, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Available, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Available, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Lost, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Lost, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Locked, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Locked, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Sold, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.Settled, [(ValueState.NotSettled, StorageState.Sold, true)]);

        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Available, false)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Available, true)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Lost, false)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Lost, true)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Locked, false)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Locked, true)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Sold, false)]);
        CreateSellerAndProducts(basar, null, ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Sold, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Available, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Available, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Lost, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Lost, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Locked, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Locked, true)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Sold, false)]);
        CreateSellerAndProducts(basar, "DE76500105171978746253", ValueState.NotSettled, [(ValueState.NotSettled, StorageState.Sold, true)]);
        CreateSellerAndProducts(basar, "AT373219569166859861", ValueState.NotSettled, [
            (ValueState.Settled, StorageState.Available, false),
            (ValueState.Settled, StorageState.Available, true),
            (ValueState.Settled, StorageState.Locked, false),
            (ValueState.Settled, StorageState.Locked, true),
            (ValueState.NotSettled, StorageState.Available, false),
            (ValueState.NotSettled, StorageState.Available, true),
            (ValueState.NotSettled, StorageState.Locked, false),
            (ValueState.NotSettled, StorageState.Locked, true)
        ]);
        CreateSellerAndProducts(basar, "DE41500105174654858252", ValueState.NotSettled, [
            (ValueState.Settled, StorageState.Sold, false),
            (ValueState.Settled, StorageState.Sold, true),
            (ValueState.Settled, StorageState.Lost, false),
            (ValueState.Settled, StorageState.Lost, true),
            (ValueState.NotSettled, StorageState.Sold, false),
            (ValueState.NotSettled, StorageState.Sold, true),
            (ValueState.NotSettled, StorageState.Lost, false),
            (ValueState.NotSettled, StorageState.Lost, true)
        ]);
        await Db.SaveChangesAsync();
    }

    private void CreateSellerAndProducts(BasarEntity basar, string? iban, ValueState valueState, params (ValueState, StorageState, bool)[] prodcuts)
    {
        SellerEntity seller = _fixture.BuildSeller()
            .With(seller => seller.IBAN, iban)
            .With(seller => seller.ValueState, valueState)
            .With(seller => seller.State, ObjectState.Enabled)
            .Create();
        AcceptSessionEntity session = _fixture.BuildAcceptSession(basar, seller)
            .Create();
        foreach ((ValueState productValueState, StorageState productStorageState, bool donateProductIfNotSold) in prodcuts)
        {
            ProductEntity product = _fixture.BuildProduct()
                .With(product => product.Session, session)
                .With(product => product.ValueState, productValueState)
                .With(product => product.StorageState, productStorageState)
                .With(product => product.DonateIfNotSold, donateProductIfNotSold)
                .Create();
            session.Products.Add(product);
        }
        Db.AcceptSessions.Add(session);
    }
}
