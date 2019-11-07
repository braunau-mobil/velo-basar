using BraunauMobil.VeloBasar.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Basar")]
    public class Basar : IModel
    {
        public int Id { get; set; }

        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Gesperrt")]
        public bool IsLocked { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Artikel Provision")]
        public decimal ProductCommission { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Artikel Rabatt")]
        public decimal ProductDiscount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Verkäufer Rabatt")]
        public decimal SellerDiscount { get; set; }
    }
}
