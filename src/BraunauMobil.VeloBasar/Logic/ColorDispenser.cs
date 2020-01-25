using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Logic
{
    public class ColorDispenser
    {
        private readonly Queue<Color> _colors = new Queue<Color>();

        public ColorDispenser()
        {
            _colors.Enqueue(Color.FromRgb(0x00, 0x7b, 0xff));
            _colors.Enqueue(Color.FromRgb(0x6c, 0x75, 0x7d));
            _colors.Enqueue(Color.FromRgb(0x28, 0xa7, 0x45));
            _colors.Enqueue(Color.FromRgb(0x17, 0xa2, 0xb8));
            _colors.Enqueue(Color.FromRgb(0xff, 0xc1, 0x07));
            _colors.Enqueue(Color.FromRgb(0xdc, 0x35, 0x45));
            _colors.Enqueue(Color.FromRgb(0xf8, 0xf9, 0xfa));
            _colors.Enqueue(Color.FromRgb(0x34, 0x3a, 0x40));
        }

        public Color Next()
        {
            var color = _colors.Dequeue();
            _colors.Enqueue(color);
            return color;
        }
    }
}
