using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
    public string TokenFormat { get; set; }

    [Required]
    public string? StatusLinkFormat { get; set; }

    public override TransactionType TransactionType => TransactionType.Acceptance;

    public string GetTokenText(SellerEntity seller)
    {
        ArgumentNullException.ThrowIfNull(seller);

        return string.Format(CultureInfo.CurrentCulture, TokenFormat, seller.Token);
    }
}
