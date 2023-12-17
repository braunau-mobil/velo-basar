using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class CookiesMock
    : IResponseCookies
    , IRequestCookieCollection
{
    private readonly Dictionary<string, string> _values = new();

    public string? this[string key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key);

            if (_values.TryGetValue(key, out string? value))
            {
                return value;
            }
            return null;
        }
    }

    public int Count => _values.Count;

    public ICollection<string> Keys => _values.Keys;

    public void Append(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        _values.Set(key, value);
    }

    public void Append(string key, string value, CookieOptions _)
        => Append(key, value);

    public bool ContainsKey(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return _values.ContainsKey(key);
    }

    public void Delete(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        _values.Remove(key);
    }

    public void Delete(string key, CookieOptions _)
        => Delete(key);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _values.GetEnumerator();

    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (_values.TryGetValue(key, out string? foundValue))
        {
            value = foundValue;
            return true;
        }

        value = null;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)_values).GetEnumerator();
}
