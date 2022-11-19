using FluentValidation;
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

    public void TrimIBAN()
    {
        if (string.IsNullOrEmpty(IBAN))
        {
            return;
        }
        IBAN = IBAN.Replace(" ", "", StringComparison.InvariantCultureIgnoreCase);
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

    private readonly VeloTexts _txt;

    public SellerEntityValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(seller => seller.FirstName)
            .NotNull()
            .WithMessage(_txt.PleaseEnterFirstName)
            .MaximumLength(MaxNameLength)
            .WithMessage(_txt.FirstNameIsTooLong(MaxNameLength));

        RuleFor(seller => seller.LastName)
            .NotNull()
            .WithMessage(_txt.PleaseEnterLastName)
            .MaximumLength(MaxNameLength)
            .WithMessage(_txt.FirstNameIsTooLong(MaxNameLength));

        RuleFor(seller => seller.Street)
            .NotNull()
            .WithMessage(_txt.PleaseEnterStreet)
            .MaximumLength(MaxStreetLength)
            .WithMessage(_txt.StreetIsTooLong(MaxStreetLength));

        RuleFor(seller => seller.City)
            .NotNull()
            .WithMessage(_txt.PleaseEnterCity)
            .MaximumLength(MaxCityLength)
            .WithMessage(_txt.CityIsTooLong(MaxCityLength));

        RuleFor(seller => seller.ZIP)
            .NotNull()
            .WithMessage(_txt.PleaseEnterZIP);

        RuleFor(seller => seller.CountryId)
            .NotEqual(0)
            .WithMessage(_txt.PleaseEnterCountry);

        RuleFor(seller => seller.PhoneNumber)
            .NotNull()
            .WithMessage(_txt.PleaseEnterPhoneNumber)
            .MaximumLength(MaxPhoneNumberLength)
            .WithMessage(_txt.PhoneNumberIsTooLong(MaxPhoneNumberLength));

        RuleFor(seller => seller.IBAN)
            .Iban()
            .WithMessage(_txt.PleaseEnterValidIBAN);

        RuleFor(seller => seller.EMail)
            .EmailAddress()
            .NotNull()
            .When(seller => seller.HasNewsletterPermission).WithMessage(_txt.PleaseEnterValidEMail);
    }
}
