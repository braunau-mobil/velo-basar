using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class WordPressStatusPushSettings
    {
        public Uri EndpointUrl { get; set; }
        public string ApiKey { get; set; }
    }
    public class VeloSettings
    {
        public VeloSettings()
        {
            WordPressStatusPushSettings = new WordPressStatusPushSettings();
            //  Euro Nominations
            Nominations = new[]
            {
                500m,
                200m,
                100m,
                50m,
                20m,
                10m,
                5m,
                2m,
                1m,
                0.5m,
                0.2m,
                0.1m,
                0.05m,
                0.02m,
                0.01m
            };
        }

        [Display(Name = "Aktiver Basar")]
        public int? ActiveBasarId { get; set; }
        [Display(Name = "Verkaufs-Status Push aktivieren")]
        public bool IsWordPressStatusPushEnabled { get; set; }
        public WordPressStatusPushSettings WordPressStatusPushSettings { get; set; }
        public IReadOnlyCollection<decimal> Nominations { get; set; }
    }  
}
