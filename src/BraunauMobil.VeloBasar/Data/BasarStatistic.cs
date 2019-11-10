using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Data
{
    public class PieChartData
    {
        public int[] DataPoints { get; set; }
        public string[] Labels { get; set; }
        public string[] BackgroundColors { get; set; }
    }

    public class BasarStatistic
    {
        public Basar Basar { get; set; }
        public PieChartData Sales { get; set; }
    }
}
