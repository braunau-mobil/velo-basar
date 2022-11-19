using Microsoft.AspNetCore.Http;

namespace BraunauMobil.VeloBasar.Cookies;

public sealed record Cookie(
      string Key
    , CookieOptions CookieOptions
);
