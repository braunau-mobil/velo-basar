using BraunauMobil.VeloBasar.Models.Interfaces;

namespace BraunauMobil.VeloBasar.Models
{
    public class Settings : IModel
    {
        public int Id { get; set; }

        public bool IsInitialized { get; set; }
    }
}
