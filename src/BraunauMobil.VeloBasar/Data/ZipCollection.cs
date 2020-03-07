using BraunauMobil.VeloBasar.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BraunauMobil.VeloBasar.Data
{
    public class ZipCollection : IEnumerable<ZipMap>
    {
        private readonly IReadOnlyCollection<Country> _countries;
        private readonly List<ZipMap> _zipMaps = new List<ZipMap>();

        public ZipCollection(IReadOnlyCollection<Country> countries)
        {
            _countries = countries;
            var type = typeof(ZipCollection);
            var assembly = type.Assembly;
            var ns = type.Namespace;

            _zipMaps.AddRange(Load(assembly, $"{ns}.ZipLists", "AUT"));
            _zipMaps.AddRange(Load(assembly, $"{ns}.ZipLists", "GER"));
        }

        public IEnumerator<ZipMap> GetEnumerator() => _zipMaps.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _zipMaps.GetEnumerator();

        private IReadOnlyCollection<ZipMap> Load(Assembly assembly, string ns, string countryIso3166Alpha3Code)
        {
            using var stream = assembly.GetManifestResourceStream($"{ns}.{countryIso3166Alpha3Code}.csv");
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            var result = new List<ZipMap>();
            var zips = new HashSet<string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var tokens = line.Split(',');

                var zip = tokens[0];
                //  To simplifiy we skip duplicates (Thank you Germany)
                if (zips.Contains(zip))
                {
                    continue;
                }
                zips.Add(zip);

                var city = tokens[1];
                result.Add(new ZipMap
                {
                    Zip = zip,
                    City = city,
                    CountryId = _countries.First(c => c.Iso3166Alpha3Code == countryIso3166Alpha3Code).Id
                });
            }

            return result;
        }
    }
}
