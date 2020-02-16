using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IColorProvider
    {
        Color Primary { get; }

        Color this[string key] { get; }
    }
}
