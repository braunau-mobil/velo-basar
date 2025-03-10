﻿using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models;

public sealed class TransactionSuccessModel
{
    public TransactionSuccessModel(TransactionEntity entity, bool openDocument = false)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        if (entity.Type == TransactionType.Cancellation)
        {
            DocumentTransactionId = entity.ParentTransaction!.Id;
            OpenDocument = openDocument && entity.ParentTransaction.CanHasDocument;
        }
        else
        {
            DocumentTransactionId = entity.Id;
            OpenDocument = openDocument && entity.CanHasDocument;
        }
    }

    public TransactionEntity Entity { get; }

    public decimal AmountGiven { get; init; }

    public int DocumentTransactionId { get; init; }

    public bool OpenDocument { get; init; }

    public bool ShowChange
    {
        get => Entity.Change.Amount != 0;
    }

    public bool ShowAmountInput
    {
        get => Entity.Type == TransactionType.Sale
            || Entity.Type == TransactionType.Unsettlement;
    }
}

public sealed class TransactionSuccessModelValidator
    : AbstractValidator<TransactionSuccessModel>
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public TransactionSuccessModelValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(model => model.AmountGiven)
            .Custom(ValidateAmountGiven);
    }

    private void ValidateAmountGiven(decimal amountGive, ValidationContext<TransactionSuccessModel> context)
    {
        TransactionSuccessModel transaction = context.InstanceToValidate;

        if (!transaction.Entity.Change.IsValid)
        {
            context.AddFailure(_localizer[VeloTexts.AmountGivenTooSmall, transaction.AmountGiven, transaction.Entity.GetSoldProductsSum()]);
        }
    }
}