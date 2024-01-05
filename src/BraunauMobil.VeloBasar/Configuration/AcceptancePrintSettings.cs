using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public sealed class AcceptancePrintSettings
    : TransactionPrintSettingsBase
{
    [Required]
    public string SignatureText { get; set; }

    [Required]
    public string SubTitle { get; set; }

    [Required]
    public string TokenTitle { get; set; }

    [Required]
    public string? StatusLinkFormat { get; set; }

    public override TransactionType TransactionType => TransactionType.Acceptance;
}
