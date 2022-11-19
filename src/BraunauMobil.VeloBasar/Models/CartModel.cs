using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;

using System.Threading;

namespace BraunauMobil.VeloBasar.Models;

public sealed class CartModel
    : AbstractActiveBasarModel
{
    public bool HasProducts { get => Products.Any(); }

    public int ProductId { get; set; }

    public IReadOnlyList<ProductEntity> Products { get; set; } = Array.Empty<ProductEntity>();
}

public sealed class CartModelValidator
    : AbstractValidator<CartModel>
{
    private readonly IProductService _productService;
    private readonly ITransactionService _transactionService;
    private readonly IVeloRouter _router;
    private readonly VeloTexts _txt;

    public CartModelValidator(IProductService productService, ITransactionService transactionService, IVeloRouter router, VeloTexts txt)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(cart => cart.ProductId)
            .CustomAsync(ValidateProductId);       
    }

    private async Task ValidateProductId(int id, ValidationContext<CartModel> context, CancellationToken cancellation)
    {
        ProductEntity? product = await _productService.FindAsync(id);

        if (product != null && product.IsAllowed(TransactionType.Sale))
        {
            return;
        }

        if (product == null)
        {
            context.AddFailure(_txt.NoProductWithIdFound(id));
        }
        else if (product.ValueState == ValueState.Settled)
        {
            context.AddFailure(_txt.ProductWasSettled);
        }
        else if (product.StorageState == StorageState.Lost)
        {
            TransactionEntity transaction = await _transactionService.GetLatestAsync(context.InstanceToValidate.ActiveBasarId, product.Id);
            context.AddFailure(_txt.ProductIsLost(transaction.Notes));
        }
        else if (product.StorageState == StorageState.Locked)
        {
            TransactionEntity transaction = await _transactionService.GetLatestAsync(context.InstanceToValidate.ActiveBasarId, product.Id);
            context.AddFailure(_txt.ProductIsLocked(transaction.Notes));
        }
        else if (product.StorageState == StorageState.Sold)
        {
            TransactionEntity sale = await _transactionService.GetLatestAsync(context.InstanceToValidate.ActiveBasarId, product.Id);
            string saleUrl = _router.Transaction.ToDetails(sale.Id);
            context.AddFailure(_txt.ProductSold(saleUrl, sale.Number));
        }
        else if (product.StorageState == StorageState.NotAccepted)
        {
            context.AddFailure(_txt.ProductNotAccepted);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
