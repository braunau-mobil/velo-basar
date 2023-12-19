using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BraunauMobil.VeloBasar.Cookies;

public class CartCookie
    : ICartCookie
{
    private readonly IRequestCookieCollection _requestCookies;
    private readonly IResponseCookies _responseCookies;

    public CartCookie(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor is null) throw new ArgumentNullException(nameof(httpContextAccessor));
        HttpContext httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentException(nameof(httpContextAccessor.HttpContext));
        _requestCookies = httpContext.Request.Cookies;
        _responseCookies = httpContext.Response.Cookies;
    }

    public CartCookie(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
    {
        _requestCookies = requestCookies ?? throw new ArgumentNullException(nameof(requestCookies));
        _responseCookies = responseCookies ?? throw new ArgumentNullException(nameof(responseCookies));
    }

    public string Key { get; } = "cart";
    
    public CookieOptions Options { get; } = new()
    {
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromDays(2),
        SameSite = SameSiteMode.Strict,
        Secure = false
    };

    public void ClearCart()
    {
        _responseCookies.Delete(Key, Options);
    }

    public IList<int> GetCart()
    {
        string? json = _requestCookies[Key];
        if (json == null)
        {
            return new List<int>();
        }
        return JsonConvert.DeserializeObject<List<int>>(json) 
            ?? new();
    }

    public void SetCart(IList<int> cart)
    {
        string? json = JsonConvert.SerializeObject(cart);
        _responseCookies.Append(Key, json, Options);
    }
}
