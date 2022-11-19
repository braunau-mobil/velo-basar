using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xan.Extensions.Tasks;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class WordPressStatusPushService
    : IStatusPushService
{
    private const int _retryDelyInSeconds = 2;

    private readonly ILogger<WordPressStatusPushService> _logger;
    private readonly WordPressStatusPushSettings _settings;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly VeloDbContext _db;
    private readonly HttpClient _httpClient;
    private readonly VeloTexts _txt;

    public WordPressStatusPushService(IOptions<WordPressStatusPushSettings>
        settings, VeloTexts txt, IBackgroundTaskQueue taskQueue, ILogger<WordPressStatusPushService> logger, VeloDbContext db, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _settings = settings.Value;
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
        _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task PushAwayAsync(TransactionEntity transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (!_settings.Enabled)
        {
            return;
        }

        BasarEntity basar = transaction.Basar;

        foreach (IGrouping<SellerEntity, ProductEntity> group in transaction.Products.GetProducts().GroupBy(p => p.Session.Seller))
        {
            SellerEntity seller = group.Key;
            if (seller == null)
            {
                continue;
            }
            if (seller.Token == null)
            {
                throw new InvalidOperationException($"Seller with ID {seller.Id} does not have a token.");
            }

            string html = await GetStatesList(transaction.Basar.Id, seller.Id);

            _taskQueue.QueueBackgroundWorkItem(async token => await PostStatusAsync(seller.Token, html, token));
        }
    }

    private async Task<string> GetStatesList(int basarId, int sellerId)
    {
        IReadOnlyList<ProductEntity> products = await _db.Products.GetForSellerAsync(basarId, sellerId);
        Dictionary<string, IReadOnlyList<string>> statusMap = new()
        {
            {
                _txt.Sold,
                products.Where(p => p.ValueState == ValueState.NotSettled && (p.StorageState == StorageState.Sold || p.StorageState == StorageState.Lost)).Select(p => ProductInfo(p)).ToArray()
            },
            {
                _txt.NotSold,
                products.Where(p => p.ValueState == ValueState.NotSettled && (p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked)).Select(p => ProductInfo(p)).ToArray()
            },
            {
                _txt.Settled,
                products.Where(p => p.ValueState == ValueState.Settled && (p.StorageState == StorageState.Sold || p.StorageState == StorageState.Lost)).Select(p => ProductInfo(p)).ToArray()
            },
            {
                _txt.NotSettled,
                products.Where(p => p.ValueState == ValueState.Settled && (p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked)).Select(p => ProductInfo(p)).ToArray()
            }
        };

        StringBuilder html = new();
        foreach (KeyValuePair<string, IReadOnlyList<string>> statusGroup in statusMap)
        {
            if (statusGroup.Value.Count > 0)
            {
                html.Append("<h5>").Append(statusGroup.Key).AppendLine("</h5>");
                html.AppendLine("<ul>");
                foreach (string info in statusGroup.Value)
                {
                    html.Append("<li>").Append(info).AppendLine("</li>");
                }
                html.AppendLine("</ul>");
            }
        }

        return html.ToString();
    }

    private async Task PostStatusAsync(string accessid, string saletext, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (await PostStatusAsync(accessid, saletext))
            {
                break;
            }

            _logger.LogWarning("Retry PostStatusAsync in {_retryDelyInSeconds} seconds.", _retryDelyInSeconds);
            await Task.Delay(_retryDelyInSeconds * 1000, token);
        }
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
    private async Task<bool> PostStatusAsync(string accessid, string saletext)
    {
        try
        {
            var content = new
            {
                accessid,
                saletext
            };
            string json = JsonConvert.SerializeObject(content);

            using StringContent body = new (json, Encoding.UTF8, "application/json");
            body.Headers.Add("bm-velobasar-api-token", _settings.ApiKey);

            if (string.IsNullOrEmpty(_settings.EndpointUrl))
            {
                throw new InvalidOperationException($"{nameof(_settings.EndpointUrl)} not set in configuration.");
            }

            using HttpResponseMessage response = await _httpClient.PostAsync(new Uri(_settings.EndpointUrl), body);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "PostStatusAsync failed due to HTTP error.");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "PostStatusAsync failed due to Task cancelled exception.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostStatusAsync failed due to general error.");
        }
        return false;
    }

    private static string ProductInfo(ProductEntity product)
        => $"{product.Brand.Name} - {product.Type.Name}<br/>{product.Description} - {product.Price:C}";
}
