using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models;

public sealed class SelectSaleModel
    : AbstractActiveBasarModel
{
    public int SaleNumber { get; set; }

    public TransactionEntity? Sale { get; set; }
}

public sealed class SelectSaleModelValidator
    : AbstractValidator<SelectSaleModel>
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public SelectSaleModelValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(cart => cart.SaleNumber)
            .Custom(ValidateSaleNumber);
    }

    private void ValidateSaleNumber(int number, ValidationContext<SelectSaleModel> context)
    {
        if (context.InstanceToValidate.Sale == null)
        {
            context.AddFailure(_localizer[VeloTexts.NoSaleFoundWithNumber, number]);
        }
        else if (!context.InstanceToValidate.Sale.Products.Any(pt => pt.Product.IsAllowed(TransactionType.Cancellation)))
        {
            context.AddFailure(_localizer[VeloTexts.AllProductsOfSaleAlreadyCancelledOrSettled]);
        }
    }
}
