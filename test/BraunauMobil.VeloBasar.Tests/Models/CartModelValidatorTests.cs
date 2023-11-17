using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests.Models;

public class CartModelValidatorTests
{
    private readonly Mock<IProductService> _productService = new();
    private readonly Mock<ITransactionService> _transactionService = new();
    private readonly Mock<IVeloRouter> _router = new();
    private readonly CartModelValidator _sut;

    public CartModelValidatorTests()
    {
        _sut = new CartModelValidator(_productService.Object, _transactionService.Object, _router.Object, Helpers.CreateActualLocalizer());
    }

    private void VerifyNoOtherCalls()
    {
        _productService.VerifyNoOtherCalls();
        _transactionService.VerifyNoOtherCalls();
        _router.VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsAllowedForCart(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;

        _productService.Setup(s => s.FindAsync(product.Id))
            .ReturnsAsync(product);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsNotFound(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsSettled(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.NotAccepted;
        product.ValueState = ValueState.Settled;
        _productService.Setup(s => s.FindAsync(product.Id))
            .ReturnsAsync(product);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsLost(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Lost;
        product.ValueState = ValueState.NotSettled;
        _productService.Setup(s => s.FindAsync(product.Id))
            .ReturnsAsync(product);

        Fixture fixture = new();
        TransactionEntity transaction = fixture.BuildTransaction().Create();
        _transactionService.Setup(_ => _.GetLatestAsync(cart.ActiveBasarId, product.Id))
            .ReturnsAsync(transaction);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());
        _transactionService.Verify(s => s.GetLatestAsync(cart.ActiveBasarId, product.Id), Times.Once());

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsLocked(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Locked;
        product.ValueState = ValueState.NotSettled;
        _productService.Setup(s => s.FindAsync(product.Id))
            .ReturnsAsync(product);

        Fixture fixture = new();
        TransactionEntity transaction = fixture.BuildTransaction().Create();
        _transactionService.Setup(_ => _.GetLatestAsync(cart.ActiveBasarId, product.Id))
            .ReturnsAsync(transaction);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());
        _transactionService.Verify(s => s.GetLatestAsync(cart.ActiveBasarId, product.Id), Times.Once());

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsSold(CartModel cart, ProductEntity product, string url)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.Sold;
        product.ValueState = ValueState.NotSettled;
        _productService.Setup(s => s.FindAsync(product.Id))
            .ReturnsAsync(product);

        Fixture fixture = new();
        TransactionEntity transaction = fixture.BuildTransaction().Create();
        _transactionService.Setup(_ => _.GetLatestAsync(cart.ActiveBasarId, product.Id))
            .ReturnsAsync(transaction);

        _router.Setup(r => r.Transaction.ToDetails(transaction.Id))
            .Returns(url);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());
        _transactionService.Verify(s => s.GetLatestAsync(cart.ActiveBasarId, product.Id), Times.Once());
        _router.Verify(r => r.Transaction.ToDetails(transaction.Id), Times.Once());

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductIsNotAccepted(CartModel cart, ProductEntity product)
    {
        //  Arrange
        cart.ProductId = product.Id;

        product.StorageState = StorageState.NotAccepted;
        product.ValueState = ValueState.NotSettled;
        _productService.Setup(s => s.FindAsync(product.Id))
            .ReturnsAsync(product);

        //  Act
        ValidationResult result = await _sut.ValidateAsync(cart);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        _productService.Verify(s => s.FindAsync(product.Id), Times.Once());

        VerifyNoOtherCalls();
    }
}
