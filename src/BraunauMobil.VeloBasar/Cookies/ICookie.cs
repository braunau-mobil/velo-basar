using Microsoft.AspNetCore.Http;

namespace BraunauMobil.VeloBasar.Cookies;

public interface ICookie
{
    string Key { get; }

    CookieOptions Options { get; }
}
