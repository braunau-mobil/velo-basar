using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public sealed class SalePrintSettings
    : TransactionPrintSettingsBase
{
    [Required]
    public string FooterText { get; set; }

    [Required]
    public string HintText { get; set; }

    [Required]
    public string SellerInfoText { get; set; }

    [Required]
    public string SubTitle { get; set; }

    public override TransactionType TransactionType => TransactionType.Sale;
}
