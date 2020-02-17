using System;
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
        }

        [Display(Name = "Aktiver Basar")]
        public int? ActiveBasarId { get; set; }
        [Display(Name = "Verkaufs-Status Push aktivieren")]
        public bool IsWordPressStatusPushEnabled { get; set; }
        public WordPressStatusPushSettings WordPressStatusPushSettings { get; set; }
    }  
}
