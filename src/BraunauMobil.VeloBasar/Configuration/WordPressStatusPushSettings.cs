using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

public sealed class WordPressStatusPushSettings
{
    public string? ApiKey { get; set; }

    [Required]
    public bool Enabled { get; set; }

    public string? EndpointUrl { get; set; }    
}
