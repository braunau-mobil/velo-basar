using BraunauMobil.VeloBasar.BusinessLogic;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public sealed class ColorProviderMock
    : IColorProvider
{
    public static readonly Color Black = Color.FromRgb(0, 0, 0);

    public Color Primary { get => Black; }

    public Color this[string key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key);

            return Black;
        }
    }
}
