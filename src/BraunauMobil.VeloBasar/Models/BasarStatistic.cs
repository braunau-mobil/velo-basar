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
    public class ChartDataPoint
    {
        public decimal Value { get; set; }
        public string Label { get; set; }
        public Color Color { get; set; }
    }
    public class BasarStatistic
    {
        public Basar Basar { get; set; }
        public int SellerCount { get; set; }
        public int AcceptedProductsCount { get; set; }
        public ChartDataPoint[] AcceptedProductsByCount { get; set; }
        public decimal AcceptedProductsAmount { get; set; }
        public ChartDataPoint[] AcceptedProductsByAmount { get; set; }
        public int SoldProductsCount { get; set; }
        public ChartDataPoint[] SoldProductsByCount { get; set; }
        public decimal SoldProductsAmount { get; set; }
        public ChartDataPoint[] SoldProductsByAmount { get; set; }
        public int GoneProductsCount { get; set; }
        public int LockedProductsCount { get; set; }
        public ChartDataPoint[] PriceDistribution { get; set; }
    }
}
