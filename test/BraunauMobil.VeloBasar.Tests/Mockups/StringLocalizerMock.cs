using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class StringLocalizerMock
    : IStringLocalizer<SharedResources>

{
    public LocalizedString this[string name]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(name);

            return new (name, name);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(arguments);

            string value = $"{name}_{string.Join('_', arguments)}";
            return new(name, value);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => throw new NotImplementedException();
}
