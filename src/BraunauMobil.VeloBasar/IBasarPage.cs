using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar
{
    public interface IBasarPage
    {
        Basar Basar { get; }
        string MyPath { get; }
    }
}
