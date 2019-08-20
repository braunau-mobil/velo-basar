using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum TransactionType
    {
        Acceptance,
        Cancellation,
        Sale,
        Settlement,
    };

    public abstract class TransactionBase : BasarModel
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
