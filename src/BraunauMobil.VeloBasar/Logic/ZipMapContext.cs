using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.Logic
{
    public class ZipMapContext : IZipMapContext
    {
        private readonly VeloRepository _db;

        public ZipMapContext(VeloRepository dbContext)
        {
            _db = dbContext;
        }

        public IDictionary<int, IDictionary<string, string>> GetMap()
        {
            var result = new Dictionary<int, IDictionary<string, string>>();
            foreach (var zipGroup in _db.ZipMap.ToList().GroupBy(zm => zm.CountryId))
            {
                result.Add(zipGroup.Key, zipGroup.ToDictionary(zm => zm.Zip, zm => zm.City));
            }
            return result;
        }
    }
}
