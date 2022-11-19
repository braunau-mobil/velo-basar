using FluentValidation;

namespace BraunauMobil.VeloBasar.Models;

public sealed class TransactionSuccessModel
{
    public TransactionSuccessModel(TransactionEntity entity)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        if (entity.Type == TransactionType.Cancellation)
        {
            DocumentTransactionId = entity.ParentTransaction!.Id;
        }
        else
        {
            DocumentTransactionId = entity.Id;
        }
    }

    public TransactionEntity Entity { get; }

    public decimal  AmountGiven { get; init; }

    public int DocumentTransactionId { get; init; }

    public bool OpenDocument { get; set; }

    public bool ShowChange
    {
        get => Entity.Change.Amount != 0;
    }

    public bool ShowAmountInput
    {
        get => Entity.Type == TransactionType.Sale;
    }
}

public sealed class TransactionSuccessModelValidator
    : AbstractValidator<TransactionSuccessModel>
{
    private readonly VeloTexts _txt;

    public TransactionSuccessModelValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(model => model.AmountGiven)
            .Custom(ValidateAmountGiven);
    }

    private void ValidateAmountGiven(decimal amountGive, ValidationContext<TransactionSuccessModel> context)
    {
        TransactionSuccessModel transaction = context.InstanceToValidate;

        if (!transaction.Entity.Change.IsValid)
        {
            context.AddFailure(_txt.AmountGivenTooSmall(transaction.AmountGiven, transaction.Entity.GetSoldProductsSum()));
        }
    }
}