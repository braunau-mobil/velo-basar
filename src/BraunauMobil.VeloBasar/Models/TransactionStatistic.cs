using BraunauMobil.VeloBasar.Models;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public class TransactionStatistic
    {
        public int ProductCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public ProductsTransaction Transaction { get; set; }
    }
}
