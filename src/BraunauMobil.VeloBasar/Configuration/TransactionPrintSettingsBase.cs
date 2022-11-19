using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public abstract class TransactionPrintSettingsBase
{
    [Required]
    public string TitleFormat { get; set; }

    public abstract TransactionType TransactionType { get; }
}
