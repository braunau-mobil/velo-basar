using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.Extensions.Localization;
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
    private readonly ITransactionRouter _router;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public CartModelValidator(IProductService productService, ITransactionService transactionService, ITransactionRouter router, IStringLocalizer<SharedResources> localizer)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

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
            context.AddFailure(_localizer[VeloTexts.NoProductWithIdFound, id]);
        }
        else if (product.ValueState == ValueState.Settled)
        {
            context.AddFailure(_localizer[VeloTexts.ProductWasSettled]);
        }
        else if (product.StorageState == StorageState.Lost)
        {
            TransactionEntity transaction = await _transactionService.GetLatestAsync(context.InstanceToValidate.BasarId, product.Id);
            context.AddFailure(_localizer[VeloTexts.ProductIsLost, transaction.Notes ?? ""]);
        }
        else if (product.StorageState == StorageState.Locked)
        {
            TransactionEntity transaction = await _transactionService.GetLatestAsync(context.InstanceToValidate.BasarId, product.Id);
            context.AddFailure(_localizer[VeloTexts.ProductIsLocked, transaction.Notes ?? ""]);
        }
        else if (product.StorageState == StorageState.Sold)
        {
            TransactionEntity sale = await _transactionService.GetLatestAsync(context.InstanceToValidate.BasarId, product.Id);
            string saleUrl = _router.ToDetails(sale.Id);
            context.AddFailure(_localizer[VeloTexts.ProductSold, saleUrl, sale.Number]);
        }
        else if (product.StorageState == StorageState.NotAccepted)
        {
            context.AddFailure(_localizer[VeloTexts.ProductNotAccepted]);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
