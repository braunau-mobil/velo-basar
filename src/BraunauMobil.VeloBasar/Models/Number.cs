namespace BraunauMobil.VeloBasar.Models
{
    public class Number : BasarModel
    {
        public TransactionType Type { get; set; }

        public int NextNumber { get; set; }
    }
}
