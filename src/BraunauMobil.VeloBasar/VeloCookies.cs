using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BraunauMobil.VeloBasar
{
    public class Cookie
    {
        public string Key { get; set; }
        public CookieOptions CookieOptions { get; set; }
    }

    public static class VeloCookies
    {
        public const int MaxAcceptanceProducts = 20;

        private static readonly Cookie _acceptanceProducts = new Cookie
        {
            Key = "acceptanceProducts_{0}",
            CookieOptions = new CookieOptions
            {
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(2)
            }
        };
        private static readonly Cookie _acceptanceProductsCount = new Cookie
        {
            Key = "acceptanceProductsCount",
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
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            for (var index = 0; index < MaxAcceptanceProducts; index++)
            {
                string key = GetAcceptanceProductCookieKey(index);
                cookies.Delete(key, _acceptanceProducts.CookieOptions);
            }
            cookies.Delete(_acceptanceProductsCount.Key, _acceptanceProductsCount.CookieOptions);
        }
        public static List<Product> GetAcceptanceProducts(this IRequestCookieCollection cookies)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));
            
            var products = new List<Product>();
            
            var countString = cookies[_acceptanceProductsCount.Key];
            if (int.TryParse(countString, out var count))
            {
                for (var index = 0; index < count; index++)
                {
                    var key = GetAcceptanceProductCookieKey(index);
                    var json = cookies[key];
                    if (string.IsNullOrEmpty(json))
                    {
                        Log.Warning("Cookie {key} is null or empty.", key);
                        continue;
                    }
                    var product = JsonConvert.DeserializeObject<Product>(json);
                    products.Add(product);
                }
            }
            
            return products;
        }
        public static void SetAcceptanceProducts(this IResponseCookies cookies, IReadOnlyList<Product> products)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));
            if (products == null) throw new ArgumentNullException(nameof(products));

            if (products.Count > MaxAcceptanceProducts)
            {
                throw new ArgumentOutOfRangeException(nameof(products), $"Cannot save more that {MaxAcceptanceProducts} to cookies.");
            }

            for (var index = 0; index < products.Count; index++)
            {
                var key = GetAcceptanceProductCookieKey(index);
                var json = JsonConvert.SerializeObject(products[index]);
                cookies.Append(key, json, _acceptanceProducts.CookieOptions);
            }
            cookies.Append(_acceptanceProductsCount.Key, products.Count.ToString(CultureInfo.InvariantCulture), _acceptanceProductsCount.CookieOptions);
        }

        public static int? GetBasarId(this IRequestCookieCollection cookies)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            var id = cookies[_basarId.Key];
            if (int.TryParse(id, out int basarId))
            {
                return basarId;
            }
            return null;
        }
        public static void SetBasarId(this IResponseCookies cookies, int basarId)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            cookies.Append(_basarId.Key, $"{basarId}");
        }

        public static void ClearCart(this IResponseCookies cookies)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            cookies.Delete(_cart.Key, _cart.CookieOptions);
        }
        public static List<int> GetCart(this IRequestCookieCollection cookies)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            var json = cookies[_cart.Key];
            if (json == null)
            {
                return new List<int>();
            }
            return JsonConvert.DeserializeObject<List<int>>(json);
        }
        public static void SetCart(this IResponseCookies cookies, IReadOnlyList<int> cart)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            var json = JsonConvert.SerializeObject(cart);
            cookies.Append(_cart.Key, json, _cart.CookieOptions);
        }

        public static int GetPageSize(this IRequestCookieCollection cookies, string key)
        {
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

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
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));

            var fullKey = $"{key}.{_pageSize.Key}";
            cookies.Append(fullKey, $"{pageSize}");
        }

        private static string GetAcceptanceProductCookieKey(int index) => string.Format(CultureInfo.InvariantCulture, _acceptanceProducts.Key, index);
    }
}
