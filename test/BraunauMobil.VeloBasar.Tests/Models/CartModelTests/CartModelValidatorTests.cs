using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.CartModelTests;

public class CartModelValidatorTests
{
    private readonly IProductService _productService = X.StrictFake<IProductService>();
    private readonly ITransactionService _transactionService = X.StrictFake<ITransactionService>();
    private readonly ITransactionRouter _router = X.StrictFake<ITransactionRouter>();
    private readonly CartModelValidator _sut;

    public CartModelValidatorTests()
    {
        _sut = new CartModelValidator(_productService, _transactionService, _router, new StringLocalizerMock<SharedResources>());
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsAllowedForCart(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;

        A.CallTo(() => _productService.FindAsync(product.Id)).Returns(product);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsNotFound(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;

        A.CallTo(() => _productService.FindAsync(product.Id)).Returns((ProductEntity?)null);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsSettled(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.NotAccepted;
        product.ValueState = ValueState.Settled;
        A.CallTo(() => _productService.FindAsync(product.Id)).Returns(product);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsLost(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Lost;
        product.ValueState = ValueState.NotSettled;
        A.CallTo(() => _productService.FindAsync(product.Id)).Returns(product);

        VeloFixture fixture = new();
        TransactionEntity transaction = fixture.Create<TransactionEntity>();
        A.CallTo(() => _transactionService.GetLatestForProductAsync(product.Id)).Returns(transaction);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _transactionService.GetLatestForProductAsync(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsLocked(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Locked;
        product.ValueState = ValueState.NotSettled;
        A.CallTo(() => _productService.FindAsync(product.Id)).Returns(product);

        VeloFixture fixture = new();
        TransactionEntity transaction = fixture.Create<TransactionEntity>();
        A.CallTo(() => _transactionService.GetLatestForProductAsync(product.Id)).Returns(transaction);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _transactionService.GetLatestForProductAsync(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsSold(CartModel cart, ProductEntity product, string url)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Sold;
        product.ValueState = ValueState.NotSettled;
        A.CallTo(() => _productService.FindAsync(product.Id)).Returns(product);

        VeloFixture fixture = new();
        TransactionEntity transaction = fixture.Create<TransactionEntity>();
        A.CallTo(() => _transactionService.GetLatestForProductAsync(product.Id)).Returns(transaction);
        A.CallTo(() => _router.ToDetails(transaction.Id)).Returns(url);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _transactionService.GetLatestForProductAsync(product.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _router.ToDetails(transaction.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductIsNotAccepted(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.NotAccepted;
        product.ValueState = ValueState.NotSettled;
        A.CallTo(() => _productService.FindAsync(product.Id)).Returns(product);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        A.CallTo(() => _productService.FindAsync(product.Id)).MustHaveHappenedOnceExactly();
    }
}
