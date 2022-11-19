using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace BraunauMobil.VeloBasar.Data;

public sealed class ZipCollection
    : IEnumerable<ZipCodeEntity>
{
    private readonly IReadOnlyList<CountryEntity> _countries;
    private readonly List<ZipCodeEntity> _zipCodes = new ();

    public ZipCollection(IReadOnlyList<CountryEntity> countries)
    {
        _countries = countries ?? throw new ArgumentNullException(nameof(countries));

        Type type = typeof(ZipCollection);
        Assembly assembly = type.Assembly;
        string? ns = type.Namespace;

        _zipCodes.AddRange(Load(assembly, $"{ns}.ZipLists", "AUT"));
        _zipCodes.AddRange(Load(assembly, $"{ns}.ZipLists", "GER"));
    }

    public IEnumerator<ZipCodeEntity> GetEnumerator() => _zipCodes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _zipCodes.GetEnumerator();

    private IReadOnlyList<ZipCodeEntity> Load(Assembly assembly, string ns, string countryIso3166Alpha3Code)
    {
        string fileName = $"{ns}.{countryIso3166Alpha3Code}.csv";

        using Stream? stream = assembly.GetManifestResourceStream($"{ns}.{countryIso3166Alpha3Code}.csv");
        if (stream == null)
        {
            throw new InvalidOperationException();
        }

        using StreamReader streamReader = new (stream, Encoding.UTF8);
        List<ZipCodeEntity> result = new ();
        HashSet<string> zips = new ();

        while (!streamReader.EndOfStream)
        {
            string? line = streamReader.ReadLine();
            if (line == null)
            {
                throw new InvalidOperationException($"ZIP-CSV {fileName} contains empty lines");
            }
            IReadOnlyList<string> tokens = line.Split(',');

            string zip = tokens[0];
            //  To simplifiy we skip duplicates (Thank you Germany)
            if (zips.Contains(zip))
            {
                continue;
            }
            zips.Add(zip);

            string city = tokens[1];
            result.Add(new ZipCodeEntity
            {
                Zip = zip,
                City = city,
                CountryId = _countries.First(c => c.Iso3166Alpha3Code == countryIso3166Alpha3Code).Id
            });
        }

        return result;
    }
}
