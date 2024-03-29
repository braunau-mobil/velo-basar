﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations.Schema;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class BasarEntity
    : AbstractCrudEntity
{
    public BasarEntity()
    {
        State = ObjectState.Disabled;
    }

    public DateTime Date { get; set; }

    public string? Location { get; set; }

    public string Name { get; set; }

    public decimal ProductCommission { get; set; }

    [NotMapped]
    public int ProductCommissionPercentage
    {
        get => (int)(ProductCommission * 100);
        set => ProductCommission = value / 100.0m;
    }
}

public sealed class BasarEntityValidator
    : AbstractValidator<BasarEntity>
{
    public BasarEntityValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterName]);

        RuleFor(b => b.ProductCommissionPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage(localizer[VeloTexts.PleaseEnterValidPercentage]);
    }
}