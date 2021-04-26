using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IBasarContext : ICrudContext<Basar>
    {
        bool Exists(int id);
        Basar GetSingle(int id);
        bool HasBasars();
    }
}
