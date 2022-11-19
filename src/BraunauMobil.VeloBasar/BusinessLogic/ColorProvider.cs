using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class ColorProvider
    : IColorProvider
{
    private static readonly IReadOnlyList<Color> _availableColors = new[]
    {
        Color.FromRgb(0x00, 0x7b, 0xff),
        Color.FromRgb(0x6c, 0x75, 0x7d),
        Color.FromRgb(0x28, 0xa7, 0x45),
        Color.FromRgb(0x17, 0xa2, 0xb8),
        Color.FromRgb(0xff, 0xc1, 0x07),
        Color.FromRgb(0xdc, 0x35, 0x45),
        Color.FromRgb(0xf8, 0xf9, 0xfa),
        Color.FromRgb(0x34, 0x3a, 0x40)
    };

    private readonly Queue<Color> _nextColors = new();
    private readonly Dictionary<string, Color> _colorMap = new();

    public ColorProvider()
    {
        _nextColors = new Queue<Color>(_availableColors);
    }

    public Color Primary
    {
        get => _availableColors[0];
    }

    public Color this[string key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key);

            if (_colorMap.TryGetValue(key, out Color color))
            {
                return color;
            }
            color = TakeNextColor();
            _colorMap.Add(key, color);
            return color;
        }
    }

    private Color TakeNextColor()
    {
        Color color = _nextColors.Dequeue();
        _nextColors.Enqueue(color);
        return color;
    }
}
