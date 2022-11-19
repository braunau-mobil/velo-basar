using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public class ApplicationSettings
{
    [Required]
    public string Culture { get; set; }

    [Required]
    public string ConnectionString { get; set; }
}
