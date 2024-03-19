using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public sealed class PrintSettings
{
    [Required]
    public AcceptancePrintSettings Acceptance { get; set; }

    [Required]
    public SalePrintSettings Sale { get; set; }

    [Required]
    public SettlementPrintSettings Settlement { get; set; }

    [Required]
    public LabelPrintSettings Label { get; set; }

    [Required]
    public Margins PageMargins { get; set; }

    public bool UseBannerFile { get; set; }

    public string BannerFilePath { get; set; }

    [Required]
    public string BannerSubtitle { get; set; }

    [Required]
    public string Website { get; set; }

    [Required]
    public int QrCodeLengthMillimeters { get; set; }
}
#nullable restore warnings

public sealed class PrintSettingsValidation
    : IValidateOptions<PrintSettings>
{
    public ValidateOptionsResult Validate(string? name, PrintSettings options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.UseBannerFile)
        {
            if (!File.Exists(options.BannerFilePath))
            {
                return ValidateOptionsResult.Fail($"{nameof(PrintSettings.BannerFilePath)}: '{options.BannerFilePath}' does not exist.");
            }
        }
        return ValidateOptionsResult.Success;
    }
}
