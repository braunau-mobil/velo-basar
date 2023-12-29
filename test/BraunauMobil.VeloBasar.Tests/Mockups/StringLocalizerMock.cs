using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class StringLocalizerMock<T>
    : IStringLocalizer<T>

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
        => throw new NotImplementedException();

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => throw new NotImplementedException();
}
