using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Data
{
    public class TransactionStatistic<T>
    {
        public int ProductCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public T Transaction { get; set; }
    }
}
