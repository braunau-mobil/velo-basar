using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class ProductEntity
    : AbstractEntity
    , IHasTimestamps
{
    public ProductEntity()
    { }

    public ProductEntity(AcceptSessionEntity session)
    {
        ArgumentNullException.ThrowIfNull(session);

        Session = session;
        SessionId = session.Id;
    }

    public string? FrameNumber { get; set; }

    public string? Color { get; set; }

    public string Brand { get; set; }

    public string Description { get; set; }

    public int TypeId { get; set; }

    public ProductTypeEntity Type { get; set; }

    public string? TireSize { get; set; }

    public decimal Price { get; set; }

    public StorageState StorageState { get; set; } = StorageState.NotAccepted;

    public ValueState ValueState { get; set; } = ValueState.NotSettled;

    public int SessionId { get; set; }

    public AcceptSessionEntity Session { get; set; }

    public bool DonateIfNotSold { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool CanEdit => (StorageState == StorageState.Available || StorageState == StorageState.NotAccepted) && ValueState == ValueState.NotSettled;

    public bool CanBeSettledWithoutSeller => StorageState != StorageState.NotAccepted && (StorageState == StorageState.Sold || StorageState == StorageState.Lost || DonateIfNotSold);

    public bool IsLost => StorageState == StorageState.Lost;
    
    public bool IsLocked => StorageState == StorageState.Locked;

    public bool WasPickedUp => ValueState == ValueState.Settled && (StorageState == StorageState.Available || StorageState == StorageState.Locked);

    public bool IsAllowed(TransactionType transactionType)
        => transactionType switch
    {
        TransactionType.Acceptance => StorageState == StorageState.NotAccepted && ValueState == ValueState.NotSettled,
        TransactionType.Cancellation => ValueState == ValueState.NotSettled && StorageState == StorageState.Sold, 
        TransactionType.Lock => StorageState == StorageState.Available && ValueState == ValueState.NotSettled,
        TransactionType.SetLost => StorageState == StorageState.Available && ValueState == ValueState.NotSettled,
        TransactionType.Unlock => (StorageState == StorageState.Locked || StorageState == StorageState.Lost) && ValueState == ValueState.NotSettled,
        TransactionType.Sale => StorageState == StorageState.Available && ValueState == ValueState.NotSettled,
        TransactionType.Settlement => (StorageState == StorageState.Available || StorageState == StorageState.Locked || StorageState == StorageState.Lost || StorageState == StorageState.Sold) && ValueState == ValueState.NotSettled,
        TransactionType.Unsettlement => (StorageState == StorageState.Available || StorageState == StorageState.Locked || StorageState == StorageState.Lost || StorageState == StorageState.Sold) && ValueState == ValueState.Settled,
        _ => throw new UnreachableException(),
    };

    public void SetState(TransactionType transactionType)
    {
        switch (transactionType)
        {
            case TransactionType.Acceptance:
                ValueState = ValueState.NotSettled;
                StorageState = StorageState.Available;
                return;
            case TransactionType.Cancellation:
                if (ValueState == ValueState.NotSettled && StorageState == StorageState.Sold)
                {
                    StorageState = StorageState.Available;
                }
                ValueState = ValueState.NotSettled;
                return;
            case TransactionType.Lock:
                StorageState = StorageState.Locked;
                return;
            case TransactionType.SetLost:
                StorageState = StorageState.Lost;
                return;
            case TransactionType.Unlock:
                StorageState = StorageState.Available;
                return;
            case TransactionType.Sale:
                StorageState = StorageState.Sold;
                return;
            case TransactionType.Settlement:
                ValueState = ValueState.Settled;
                return;
            case TransactionType.Unsettlement:
                ValueState = ValueState.NotSettled;
                return;
            default:
                throw new UnreachableException();
        }
    }
    public decimal GetCommissionedPrice(BasarEntity basar)
    {
        ArgumentNullException.ThrowIfNull(basar);

        return Price - GetCommissionAmount(basar);
    }
    public decimal GetCommissionAmount(BasarEntity basar)
    {
        ArgumentNullException.ThrowIfNull(basar);

        if (basar.ProductCommission == 0.0m)
        {
            return 0;
        }
        return Price * basar.ProductCommission;
    }
    public bool ShouldBePayedBack()
    {
        return ValueState == ValueState.NotSettled
            &&
            (StorageState == StorageState.Sold || StorageState == StorageState.Lost);
    }
    public bool ShouldBePayedOut()
    {
        return ValueState == ValueState.Settled
            &&
            (StorageState == StorageState.Sold || StorageState == StorageState.Lost);
    }
    public bool ShouldBePickedUp()
    {
        return ValueState == ValueState.Settled
            &&
            (StorageState == StorageState.Available || StorageState == StorageState.Locked);
    }
}

public sealed class ProductEntityValidator
    : AbstractValidator<ProductEntity>
{
    public const int MaxDescriptionLength = 80;

    public ProductEntityValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(product => product.Brand)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterBrand]);

        RuleFor(seller => seller.TypeId)
            .NotEqual(0)
            .WithMessage(localizer[VeloTexts.PleaseEnterProductType]);

        RuleFor(seller => seller.Description)
            .NotNull()
            .WithMessage(localizer[VeloTexts.PleaseEnterDescription])
            .MaximumLength(MaxDescriptionLength)
            .WithMessage(localizer[VeloTexts.ProductDescriptionIsTooLong, MaxDescriptionLength]);

        RuleFor(seller => seller.Price)
            .GreaterThan(0)
            .WithMessage(localizer[VeloTexts.PriceMustBeGreaterThanZero]);

        RuleFor(seller => seller.Price)
            .PrecisionScale(12, 2, true)
            .WithMessage(localizer[VeloTexts.PriceMustHavePrecisition]);
    }
}
