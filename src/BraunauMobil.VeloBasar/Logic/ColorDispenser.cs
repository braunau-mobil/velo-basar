using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Logic
{
    public class ColorDispenser
    {
        private readonly Queue<Color> _colors = new Queue<Color>();

        public ColorDispenser()
        {
            _colors.Enqueue(Color.FromRgb(255, 99, 132));
            _colors.Enqueue(Color.FromRgb(255, 159, 64));
            _colors.Enqueue(Color.FromRgb(255, 205, 86));
            _colors.Enqueue(Color.FromRgb(75, 192, 192));
            _colors.Enqueue(Color.FromRgb(54, 162, 235));
            _colors.Enqueue(Color.FromRgb(153, 102, 255));
            _colors.Enqueue(Color.FromRgb(201, 203, 207));
        }

        public Color Next()
        {
            var color = _colors.Dequeue();
            _colors.Enqueue(color);
            return color;
        }
    }
}
