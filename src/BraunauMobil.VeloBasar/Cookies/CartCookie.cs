using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BraunauMobil.VeloBasar.Cookies;

public class CartCookie
    : ICartCookie
{
    private readonly HttpContext _httpContext;

    public CartCookie(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor is null) throw new ArgumentNullException(nameof(httpContextAccessor));
        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentException(nameof(httpContextAccessor.HttpContext));
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
        _httpContext.Response.Cookies.Delete(Key, Options);
    }

    public IList<int> GetCart()
    {
        string? json = _httpContext.Request.Cookies[Key];
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
        _httpContext.Response.Cookies.Append(Key, json, Options);
    }
}
