using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class WordPressStatusPushService : IStatusPushService
    {
        private const int _retryDelyInSeconds = 2;

        private readonly IProductContext _productContext;
        private readonly ISettingsContext _settingsContext;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IBackgroundTaskQueue _taskQueue;

        public WordPressStatusPushService(IProductContext productContext, ISettingsContext settingsContext, IStringLocalizer<SharedResource> stringLocalizer, IBackgroundTaskQueue taskQueue)
        {
            _productContext = productContext;
            _settingsContext = settingsContext;
            _stringLocalizer = stringLocalizer;
            _taskQueue = taskQueue;
        }

        public async Task PushAwayAsync(Basar basar, IEnumerable<Product> products)
        {
            var settings = await _settingsContext.GetSettingsAsync();
            if (!settings.IsWordPressStatusPushEnabled)
            {
                return;
            }

            foreach (var group in products.GroupBy(p => p.Seller))
            {
                var seller = group.Key;
                var html = await GetStatesList(basar, seller);

                _taskQueue.QueueBackgroundWorkItem(async token => await PostStatusAsync(settings.WordPressStatusPushSettings, seller.Token, html, token));
            }
        }

        private async Task<string> GetStatesList(Basar basar, Seller seller)
        {
            var products = await _productContext.GetProductsForSeller(basar, seller.Id).ToArrayAsync();
            var statusMap = new Dictionary<string, IReadOnlyCollection<string>>
            {
                {
                    _stringLocalizer["Verkauft"],
                    products.Where(p => p.ValueState == ValueState.NotSettled && (p.StorageState == StorageState.Sold || p.StorageState == StorageState.Gone)).Select(p => ProductInfo(p)).ToArray()
                },
                {
                    _stringLocalizer["Nicht verkauft"],
                    products.Where(p => p.ValueState == ValueState.NotSettled && (p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked)).Select(p => ProductInfo(p)).ToArray()
                },
                {
                    _stringLocalizer["Abgerechnet"],
                    products.Where(p => p.ValueState == ValueState.Settled && (p.StorageState == StorageState.Sold || p.StorageState == StorageState.Gone)).Select(p => ProductInfo(p)).ToArray()
                },
                {
                    _stringLocalizer["Nicht abgerechnet"],
                    products.Where(p => p.ValueState == ValueState.Settled && (p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked)).Select(p => ProductInfo(p)).ToArray()
                }
            };

            var html = new StringBuilder();
            foreach (var statusGroup in statusMap)
            {
                if (statusGroup.Value.Count > 0)
                {
                    html.AppendLine($"<h5>{statusGroup.Key}</h5>");
                    html.AppendLine("<ul>");
                    foreach (var info in  statusGroup.Value)
                    {
                        html.AppendLine($"<li>{info}</li>");
                    }
                    html.AppendLine("</ul>");
                }
            }

            return html.ToString();
        }

        private static async Task PostStatusAsync(WordPressStatusPushSettings settings, string accessid, string saletext, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (await PostStatusAsync(settings, accessid, saletext))
                {
                    break;
                }

                Log.Warning($"Retry PostStatusAsync in {_retryDelyInSeconds} seconds.");
                await Task.Delay(_retryDelyInSeconds * 1000);
            }
        }

        private static async Task<bool> PostStatusAsync(WordPressStatusPushSettings settings, string accessid, string saletext)
        {
            try
            {
                var content = new
                {
                    accessid,
                    saletext
                };
                var json = JsonConvert.SerializeObject(content);
            
                using var body = new StringContent(json, Encoding.UTF8, "application/json");
                body.Headers.Add("bm-velobasar-api-token", settings.ApiKey);
            
                using var httpClient = new HttpClient();
                using var response = await httpClient.PostAsync(settings.EndpointUrl, body);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException httpRequestException)
            {
                Log.Error(httpRequestException, "PostStatusAsync failed due to HTTP error.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "PostStatusAsync failed due to general error.");
            }
            return false;
        }
        private static string ProductInfo(Product product) => $"{product.Brand.Name} - {product.Type.Name}<br/>{product.Description} - {product.Price:C}";
    }
}
