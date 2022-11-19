using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BraunauMobil.VeloBasar.Cookies;

public static class CartCookie
{
    private static readonly Cookie _cart = new(
        "cart",
        new CookieOptions
        {
            IsEssential = true,
            MaxAge = TimeSpan.FromDays(2)
        });

    public static void ClearCart(this IResponseCookies cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        cookies.Delete(_cart.Key, _cart.CookieOptions);
    }
    public static IList<int> GetCart(this IRequestCookieCollection cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        string? json = cookies[_cart.Key];
        if (json == null)
        {
            return new List<int>();
        }
        return JsonConvert.DeserializeObject<List<int>>(json) 
            ?? new();
    }
    public static void SetCart(this IResponseCookies cookies, IList<int> cart)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        string? json = JsonConvert.SerializeObject(cart);
        cookies.Append(_cart.Key, json, _cart.CookieOptions);
    }
}
