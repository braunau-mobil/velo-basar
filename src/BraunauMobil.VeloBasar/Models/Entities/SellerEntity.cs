using FluentValidation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations.Schema;
using Xan.AspNetCore.Validation;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class SellerEntity
    : AbstractCrudEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string ZIP { get; set; }

    public int CountryId { get; set; }

    public CountryEntity Country { get; set; }

    public string? EMail { get; set; }

    public bool HasNewsletterPermission { get; set; }

    public DateTime? NewsletterPermissionTimesStamp { get; set; }

    public string PhoneNumber { get; set; }

    public string? IBAN { get; set; }

    public string? BankAccountHolder { get; set; }

    public string Token { get; set; } = Guid.NewGuid().ToString();

    public ValueState ValueState { get; set; }

    public string? Comment { get; set; }

    [NotMapped]
    public string EffectiveBankAccountHolder
    {
        get
        {
            if (string.IsNullOrEmpty(BankAccountHolder))
            {
                return $"{FirstName} {LastName}";
            }
            return BankAccountHolder;
        }
    }

    public void UnifyIBAN()
    {
        if (string.IsNullOrEmpty(IBAN))
        {
            return;
        }
        //  Trim
        IBAN = IBAN.Replace(" ", "", StringComparison.InvariantCultureIgnoreCase);
        //  To upper
        IBAN = IBAN.ToUpperInvariant();
    }

    public void UpdateNewsletterPermissions(IClock clock)
    {
        ArgumentNullException.ThrowIfNull(clock);

        if (HasNewsletterPermission)
        {
            NewsletterPermissionTimesStamp = clock.GetCurrentDateTime();
        }
    }
}

public sealed class SellerEntityValidator
    : AbstractValidator<SellerEntity>
{
    public const int MaxNameLength = 100;
    public const int MaxStreetLength = 100;
    public const int MaxCityLength = 100;
    public const int MaxPhoneNumberLength = 30;

    public SellerEntityValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(seller => seller.FirstName)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterFirstName])
            .MaximumLength(MaxNameLength)
            .WithMessage(localizer[VeloTexts.FirstNameIsTooLong, MaxNameLength]);

        RuleFor(seller => seller.LastName)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterLastName])
            .MaximumLength(MaxNameLength)
            .WithMessage(localizer[VeloTexts.FirstNameIsTooLong, MaxNameLength]);

        RuleFor(seller => seller.Street)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterStreet])
            .MaximumLength(MaxStreetLength)
            .WithMessage(localizer[VeloTexts.StreetIsTooLong, MaxStreetLength]);

        RuleFor(seller => seller.City)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterCity])
            .MaximumLength(MaxCityLength)
            .WithMessage(localizer[VeloTexts.CityIsTooLong, MaxCityLength]);

        RuleFor(seller => seller.ZIP)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterZIP]);

        RuleFor(seller => seller.CountryId)
            .NotEqual(0)
            .WithMessage(localizer[VeloTexts.PleaseEnterCountry]);

        RuleFor(seller => seller.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterPhoneNumber])
            .MaximumLength(MaxPhoneNumberLength)
            .WithMessage(localizer[VeloTexts.PhoneNumberIsTooLong, MaxPhoneNumberLength]);

        RuleFor(seller => seller.IBAN)
            .Iban()
            .WithMessage(localizer[VeloTexts.PleaseEnterValidIBAN]);

        RuleFor(seller => seller.EMail)
            .EmailAddress();
    }
}
