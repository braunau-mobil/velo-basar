using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IZipMapContext
    {
        IDictionary<int, IDictionary<string, string>> GetMap();
    }
}
