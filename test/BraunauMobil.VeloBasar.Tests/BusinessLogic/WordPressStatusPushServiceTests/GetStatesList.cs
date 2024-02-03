namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.WordPressStatusPushServiceTests;

public class GetStatesList
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task NoProducts_ShouldReturnEmptyString(int basarId, int sellerId)
    {
        //  Arrange

        //  Act
        string result = await Sut.GetStatesListAsync(basarId, sellerId);

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public async Task SoldProducts_ShouldReturnHtml(BasarEntity basar, SellerEntity seller)
    {
        //  Arrange
        AcceptSessionEntity acceptSessionEntity = new()
        {
            Basar = basar,
            Seller = seller        
        };
        acceptSessionEntity.Products.Add(CreateProduct("sold", StorageState.Sold, ValueState.NotSettled));
        acceptSessionEntity.Products.Add(CreateProduct("lost", StorageState.Lost, ValueState.NotSettled));
        Db.AcceptSessions.Add(acceptSessionEntity);
        await Db.SaveChangesAsync();

        //  Act
        string result = await Sut.GetStatesListAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be("<h5>VeloBasar_Sold</h5>\r\n<ul>\r\n<li>sold_brand - sold_type<br/>sold_description - € 1,00</li>\r\n<li>lost_brand - lost_type<br/>lost_description - € 1,00</li>\r\n</ul>\r\n");
    }

    [Theory]
    [VeloAutoData]
    public async Task NotSoldProducts_ShouldReturnHtml(BasarEntity basar, SellerEntity seller)
    {
        //  Arrange
        AcceptSessionEntity acceptSessionEntity = new()
        {
            Basar = basar,
            Seller = seller
        };
        acceptSessionEntity.Products.Add(CreateProduct("available", StorageState.Available, ValueState.NotSettled));
        acceptSessionEntity.Products.Add(CreateProduct("locked", StorageState.Locked, ValueState.NotSettled));
        Db.AcceptSessions.Add(acceptSessionEntity);
        await Db.SaveChangesAsync();

        //  Act
        string result = await Sut.GetStatesListAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be("<h5>VeloBasar_NotSold</h5>\r\n<ul>\r\n<li>available_brand - available_type<br/>available_description - € 1,00</li>\r\n<li>locked_brand - locked_type<br/>locked_description - € 1,00</li>\r\n</ul>\r\n");
    }

    [Theory]
    [VeloAutoData]
    public async Task SettledProducts_ShouldReturnHtml(BasarEntity basar, SellerEntity seller)
    {
        //  Arrange
        AcceptSessionEntity acceptSessionEntity = new()
        {
            Basar = basar,
            Seller = seller
        };
        acceptSessionEntity.Products.Add(CreateProduct("sold_settled", StorageState.Available, ValueState.Settled));
        acceptSessionEntity.Products.Add(CreateProduct("lost_settled", StorageState.Lost, ValueState.Settled));
        Db.AcceptSessions.Add(acceptSessionEntity);
        await Db.SaveChangesAsync();

        //  Act
        string result = await Sut.GetStatesListAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be("<h5>VeloBasar_Settled</h5>\r\n<ul>\r\n<li>lost_settled_brand - lost_settled_type<br/>lost_settled_description - € 1,00</li>\r\n</ul>\r\n<h5>VeloBasar_PickedUp</h5>\r\n<ul>\r\n<li>sold_settled_brand - sold_settled_type<br/>sold_settled_description - € 1,00</li>\r\n</ul>\r\n");
    }

    [Theory]
    [VeloAutoData]
    public async Task PickedUpProducts_ShouldReturnHtml(BasarEntity basar, SellerEntity seller)
    {
        //  Arrange
        AcceptSessionEntity acceptSessionEntity = new()
        {
            Basar = basar,
            Seller = seller
        };
        acceptSessionEntity.Products.Add(CreateProduct("available_settled", StorageState.Available, ValueState.Settled));
        acceptSessionEntity.Products.Add(CreateProduct("locked_settled", StorageState.Locked, ValueState.Settled));
        Db.AcceptSessions.Add(acceptSessionEntity);
        await Db.SaveChangesAsync();

        //  Act
        string result = await Sut.GetStatesListAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be("<h5>VeloBasar_PickedUp</h5>\r\n<ul>\r\n<li>available_settled_brand - available_settled_type<br/>available_settled_description - € 1,00</li>\r\n<li>locked_settled_brand - locked_settled_type<br/>locked_settled_description - € 1,00</li>\r\n</ul>\r\n");
    }

    [Theory]
    [VeloAutoData]
    public async Task AllKindsOfProducts_ShouldReturnHtml(BasarEntity basar, SellerEntity seller)
    {
        //  Arrange
        AcceptSessionEntity acceptSessionEntity = new()
        {
            Basar = basar,
            Seller = seller
        };

        acceptSessionEntity.Products.Add(CreateProduct("sold", StorageState.Sold, ValueState.NotSettled));
        acceptSessionEntity.Products.Add(CreateProduct("lost", StorageState.Lost, ValueState.NotSettled));
        acceptSessionEntity.Products.Add(CreateProduct("available", StorageState.Available, ValueState.NotSettled));
        acceptSessionEntity.Products.Add(CreateProduct("locked", StorageState.Locked, ValueState.NotSettled));
        acceptSessionEntity.Products.Add(CreateProduct("sold_settled", StorageState.Available, ValueState.Settled));
        acceptSessionEntity.Products.Add(CreateProduct("lost_settled", StorageState.Lost, ValueState.Settled));
        acceptSessionEntity.Products.Add(CreateProduct("available_settled", StorageState.Available, ValueState.Settled));
        acceptSessionEntity.Products.Add(CreateProduct("locked_settled", StorageState.Locked, ValueState.Settled));
        Db.AcceptSessions.Add(acceptSessionEntity);
        await Db.SaveChangesAsync();

        //  Act
        string result = await Sut.GetStatesListAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be("<h5>VeloBasar_Sold</h5>\r\n<ul>\r\n<li>sold_brand - sold_type<br/>sold_description - € 1,00</li>\r\n<li>lost_brand - lost_type<br/>lost_description - € 1,00</li>\r\n</ul>\r\n<h5>VeloBasar_NotSold</h5>\r\n<ul>\r\n<li>available_brand - available_type<br/>available_description - € 1,00</li>\r\n<li>locked_brand - locked_type<br/>locked_description - € 1,00</li>\r\n</ul>\r\n<h5>VeloBasar_Settled</h5>\r\n<ul>\r\n<li>lost_settled_brand - lost_settled_type<br/>lost_settled_description - € 1,00</li>\r\n</ul>\r\n<h5>VeloBasar_PickedUp</h5>\r\n<ul>\r\n<li>sold_settled_brand - sold_settled_type<br/>sold_settled_description - € 1,00</li>\r\n<li>available_settled_brand - available_settled_type<br/>available_settled_description - € 1,00</li>\r\n<li>locked_settled_brand - locked_settled_type<br/>locked_settled_description - € 1,00</li>\r\n</ul>\r\n");
    }

    private ProductEntity CreateProduct(string prefix, StorageState storageState, ValueState valueState)
    {
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.Brand, $"{prefix}_brand")
            .With(product => product.Description, $"{prefix}_description")
            .With(product => product.StorageState, storageState)
            .With(product => product.ValueState, valueState)
            .Create();

        product.Type.Name = $"{prefix}_type";
        product.Price = 1;

        return product;
    }
}
