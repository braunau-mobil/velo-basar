using BraunauMobil.VeloBasar.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Basar")]
    public class Basar : IModel
    {
        public int Id { get; set; }
        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Ort")]
        public string Location { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Artikel Provision")]
        public decimal ProductCommission { get; set; }

        [Display(Name = "Artikel Provision in Prozent")]
        [Range(0, 100, ErrorMessage = "Bitte einen Wert zwischen 0 und 100 % eingeben")]
        [NotMapped]
        public int ProductCommissionPercentage
        {
            get => (int)(ProductCommission * 100);
            set => ProductCommission = value / 100.0m;
        }

        public string GetDateText()
        {
            var type = typeof(Basar);
            var propertyInfo = type.GetProperty(nameof(Date));
            var displayFormatAttribute = propertyInfo.GetCustomAttribute<DisplayFormatAttribute>();
            return string.Format(CultureInfo.CurrentCulture, displayFormatAttribute.DataFormatString, Date);
        }
        public string GetLocationAndDateText()
        {
            return $"{Location}, {GetDateText()}";
        }
    }
}
