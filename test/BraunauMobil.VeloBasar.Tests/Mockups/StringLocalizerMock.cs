using Microsoft.Extensions.Localization;
using System.Text;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class StringLocalizerMock(IFormatProvider formatProvider)
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

            StringBuilder argsString = new();
            foreach (object argument in arguments)
            {
                string argString = string.Format(formatProvider, "{0}", argument);
                if (argsString.Length > 0)
                {
                    argsString.Append('_');
                }
                argsString.Append(argString);
            }

            string value = $"{name}_{argsString}";
            return new(name, value);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => throw new NotImplementedException();
}
