using BraunauMobil.VeloBasar.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models.Base
{
    public class TransactionModel : BasarModel, IModel
    {
        public int Id { get; set; }

        [Display(Name = "Nummer")]
        public int Number { get; set; }

        public int? DocumentId { get; set; }

        [Display(Name = "Datum und Uhrzeit")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime TimeStamp { get; set; }

        public TransactionType Type { get; set; }

        public string Notes { get; set; }
    }
}
