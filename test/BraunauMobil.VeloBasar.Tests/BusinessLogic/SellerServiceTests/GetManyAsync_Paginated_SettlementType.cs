using BraunauMobil.VeloBasar.Parameters;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Mvc.Crud;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class GetManyAsync_Paginated_SettlementType
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task OnSite_ShouldBeCorrect(BasarEntity basar)
    {
        //  Arrange
        await InsertTestData(basar);
        SellerListParameter parameter = new()
        {
            BasarId = basar.Id,
            PageSize = 10,
            PageIndex = 1,
            SettlementType = SellerSettlementType.OnSite
        };

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.TotalItemCount.Should().Be(17);
    }


    [Theory]
    [VeloAutoData]
    public async Task Remote_ShouldBeCorrect(BasarEntity basar)
    {
        //  Arrange
        await InsertTestData(basar);
        SellerListParameter parameter = new()
        {
            BasarId = basar.Id,
            PageSize = 10,
            PageIndex = 1,
            SettlementType = SellerSettlementType.Remote
        };

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.TotalItemCount.Should().Be(17);
    }

    private async Task InsertTestData(BasarEntity basar)
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
