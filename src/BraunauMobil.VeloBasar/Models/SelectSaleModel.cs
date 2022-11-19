using FluentValidation;

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
    private readonly VeloTexts _txt;

    public SelectSaleModelValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(cart => cart.SaleNumber)
            .Custom(ValidateSaleNumber);
    }

    private void ValidateSaleNumber(int number, ValidationContext<SelectSaleModel> context)
    {
        if (context.InstanceToValidate.Sale == null)
        {
            context.AddFailure(_txt.NoSaleFoundWithNumber(number));
        }
        else if (!context.InstanceToValidate.Sale.Products.Any(pt => pt.Product.IsAllowed(TransactionType.Cancellation)))
        {
            context.AddFailure(_txt.AllProductsOfSaleAlreadyCancelledOrSettled);
        }
    }
}
