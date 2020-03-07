using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar
{
    public class Cookie
    {
        public string Key { get; set; }
        public CookieOptions CookieOptions { get; set; }
    }

    public static class VeloCookies
    {
        private static readonly Cookie _acceptanceProducts = new Cookie
        {
            Key = "acceptanceProducts",
            CookieOptions = new CookieOptions
            {
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(2)
            }
        };
        private static readonly Cookie _basarId = new Cookie
        {
            Key = "basarId",
            CookieOptions = new CookieOptions
            {
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(2)
            }
        };
        private static readonly Cookie _cart = new Cookie
        {
            Key = "cart",
            CookieOptions = new CookieOptions
            {
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(2)
            }
        };
        private static readonly Cookie _pageSize = new Cookie
        {
            Key = "pageSize",
            CookieOptions = new CookieOptions
            {
                IsEssential = false,
                MaxAge = TimeSpan.FromDays(2)
            }
        };

        public static void ClearAcceptanceProducts(this IResponseCookies cookies)
        {
            Contract.Requires(cookies != null);

            cookies.Delete(_acceptanceProducts.Key, _acceptanceProducts.CookieOptions);
        }
        public static List<Product> GetAcceptanceProducts(this IRequestCookieCollection cookies)
        {
            Contract.Requires(cookies != null);

            var json = cookies[_acceptanceProducts.Key];
            if (json == null)
            {
                return new List<Product>();
            }
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }
        public static void SetAcceptanceProducts(this IResponseCookies cookies, IReadOnlyList<Product> products)
        {
            Contract.Requires(cookies != null);

            var json = JsonConvert.SerializeObject(products);
            cookies.Append(_acceptanceProducts.Key, json);
        }

        public static int? GetBasarId(this IRequestCookieCollection cookies)
        {
            Contract.Requires(cookies != null);

            var id = cookies[_basarId.Key];
            if (int.TryParse(id, out int basarId))
            {
                return basarId;
            }
            return null;
        }
        public static void SetBasarId(this IResponseCookies cookies, int basarId)
        {
            Contract.Requires(cookies != null);

            cookies.Append(_basarId.Key, $"{basarId}");
        }

        public static void ClearCart(this IResponseCookies cookies)
        {
            Contract.Requires(cookies != null);

            cookies.Delete(_cart.Key, _cart.CookieOptions);
        }
        public static List<int> GetCart(this IRequestCookieCollection cookies)
        {
            Contract.Requires(cookies != null);

            var json = cookies[_cart.Key];
            if (json == null)
            {
                return new List<int>();
            }
            return JsonConvert.DeserializeObject<List<int>>(json);
        }
        public static void SetCart(this IResponseCookies cookies, IReadOnlyList<int> cart)
        {
            Contract.Requires(cookies != null);

            var json = JsonConvert.SerializeObject(cart);
            cookies.Append(_cart.Key, json, _cart.CookieOptions);
        }

        public static int GetPageSize(this IRequestCookieCollection cookies, string key)
        {
            Contract.Requires(cookies != null);

            var fullKey = $"{key}.{_pageSize.Key}";
            var id = cookies[fullKey];
            if (int.TryParse(id, out int pageSize))
            {
                return pageSize;
            }
            return 5;
        }
        public static void SetPageSize(this IResponseCookies cookies, string key, int pageSize)
        {
            Contract.Requires(cookies != null);

            var fullKey = $"{key}.{_pageSize.Key}";
            cookies.Append(fullKey, $"{pageSize}");
        }
    }
}
