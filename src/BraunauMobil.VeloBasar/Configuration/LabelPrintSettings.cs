using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public sealed class LabelPrintSettings
{
    [Required]
    public int WidthInMillimeters { get; set; }
    
    [Required]
    public int HeightInMillimeters { get; set; }
    
    [Required]
    public Margins Margins { get; set; }
    
    [Required]
    public int MaxDescriptionLength { get; set; }

    [Required]
    public string TitleFormat { get; set; }
}
