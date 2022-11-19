using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Models;

public sealed record ChartDataPoint(
      decimal Value
    , string Label
    , Color Color
);
