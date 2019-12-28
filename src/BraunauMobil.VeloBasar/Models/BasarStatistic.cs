namespace BraunauMobil.VeloBasar.Models
{
    public class Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color
            {
                R = r,
                G = g,
                B = b
            };
        }
    }
    public class PieChartDataPoint
    {
        public decimal Value { get; set; }
        public string Label { get; set; }
        public Color Color { get; set; }
    }
    public class LineChartDataPoint
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }
    public class BasarStatistic
    {
        public int AcceptedProductsCount { get; set; }
        public decimal AcceptedProductsAmount { get; set; }
        public int SoldProductsCount { get; set; }
        public decimal SoldProductsAmount { get; set; }

        public Basar Basar { get; set; }
        public PieChartDataPoint[] AcceptedProductsByCount { get; set; }
        public PieChartDataPoint[] AcceptedProductsByAmount { get; set; }
        public PieChartDataPoint[] SoldProductsByCount { get; set; }
        public PieChartDataPoint[] SoldProductsByAmount { get; set; }

        public LineChartDataPoint[] PriceDistribution { get; set; }
    }
}
