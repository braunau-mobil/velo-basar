using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models.Base
{
    public abstract class Transaction : BasarModel
    {
        public int Id { get; set; }

        [Display(Name = "Nummer")]
        public int Number { get; set; }

        public int? DocumentId { get; set; }

        [Display(Name = "Datum und Uhrzeit")]
        public DateTime TimeStamp { get; set; }

        public abstract TransactionType Type { get; }
    }
}
