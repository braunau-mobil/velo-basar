using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public sealed class SettlementPrintSettings
    : TransactionPrintSettingsBase
{
    [Required]
    public string ConfirmationText { get; set; }

    [Required]
    public string SignatureText { get; set; }

    [Required]
    public string SoldTitle { get; set; }

    [Required]
    public string NotSoldTitle { get; set; }

    [Required]
    public string BankTransactionTextFormat { get; set; }

    public override TransactionType TransactionType => TransactionType.Settlement;
}
