using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class ColorProvider
    : IColorProvider
{
    private readonly Queue<Color> _nextColors;
    private readonly Dictionary<string, Color> _colorMap = [];

    public ColorProvider(IOptions<ApplicationSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _nextColors = [];
        bool first = true;
        foreach (string colorString in settings.Value.Colors)
        {
            if (Color.TryParse(colorString, out Color color))
            {
                if (first)
                {
                    Primary = color;
                    first = false;
                }

                _nextColors.Enqueue(color);
            }
            else
            {
                throw new InvalidOperationException($"Cannot parse color {colorString}.");
            }
        }
    }

    public Color Primary { get; }

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
