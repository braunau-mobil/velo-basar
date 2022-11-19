using System.ComponentModel.DataAnnotations;

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

    public string? BannerFilePath { get; set; }

    [Required]
    public string BannerSubtitle { get; set; }

    [Required]
    public string Website { get; set; }

    [Required]
    public int QrCodeLengthMillimeters { get; set; }
}
