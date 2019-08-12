using BraunauMobil.VeloBasar.Models;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Data
{
    public class AcceptanceStatistic
    {
        public int ProductCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public Acceptance Acceptance { get; set; }
    }
}
