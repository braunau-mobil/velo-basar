using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public class Cookie
    {
        public string Key { get; set; }
        public CookieOptions CookieOptions { get; set; }
    }

    public static class Cookies
    {
        private static readonly Cookie _cart = new Cookie
        {
            Key = "cart",
            CookieOptions = new CookieOptions
            {
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(2)
            }
        };

        public static void ClearCart(this IResponseCookies cookies)
        {
            cookies.Delete(_cart.Key, _cart.CookieOptions);
        }
        public static IList<int> GetCart(this IRequestCookieCollection cookies)
        {
            var json = cookies[_cart.Key];
            if (json == null)
            {
                return new List<int>();
            }
            return JsonConvert.DeserializeObject<List<int>>(json);
        }
        public static void SetCart(this IResponseCookies cookies, IList<int> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            cookies.Append(_cart.Key, json, _cart.CookieOptions);
        }
    }
}
