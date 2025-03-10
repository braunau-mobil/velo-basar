﻿using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions.Tasks;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class WordPressStatusPushService
    : IStatusPushService
{
    private readonly ILogger<WordPressStatusPushService> _logger;
    private readonly WordPressStatusPushSettings _settings;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly VeloDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IFormatProvider _formatProvider;

    public WordPressStatusPushService(IOptions<WordPressStatusPushSettings>
        settings, IStringLocalizer<SharedResources> localizer, IBackgroundTaskQueue taskQueue, ILogger<WordPressStatusPushService> logger, VeloDbContext db, IHttpClientFactory httpClientFactory, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _settings = settings.Value;
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
    }

    public bool IsEnabled { get => _settings.Enabled; }

    public async Task PushSellerAsync(int basarId, int sellerId)
    {
        SellerEntity seller = await _db.Sellers.FirstByIdAsync(sellerId);
        string html = await GetStatesListAsync(basarId, seller.Id);

        _taskQueue.QueueBackgroundWorkItem(async token => await PostStatusAsync(seller.Token, html, token));

        _logger.LogInformation("Queued status push for {AccessId}", seller.Token);
    }

    public async Task<string> GetStatesListAsync(int basarId, int sellerId)
    {
        IReadOnlyList<ProductEntity> products = await _db.Products.GetForBasarAndSellerAsync(basarId, sellerId);
        Dictionary<string, IReadOnlyList<string>> statusMap = new()
        {
            {
                _localizer[VeloTexts.Sold],
                products.Where(p => p.ValueState == ValueState.NotSettled && (p.StorageState == StorageState.Sold || p.StorageState == StorageState.Lost)).Select(p => ProductInfo(p)).ToArray()
            },
            {
                _localizer[VeloTexts.NotSold],
                products.Where(p => p.ValueState == ValueState.NotSettled && (p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked)).Select(p => ProductInfo(p)).ToArray()
            },
            {
                _localizer[VeloTexts.Settled],
                products.Where(p => p.ValueState == ValueState.Settled && (p.StorageState == StorageState.Sold || p.StorageState == StorageState.Lost)).Select(p => ProductInfo(p)).ToArray()
            },
            {
                _localizer[VeloTexts.PickedUp],
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

            _logger.LogWarning("Retry PostStatusAsync in {_retryDelyInSeconds} seconds.", _settings.RetryDelayInSeconds);
            await Task.Delay(_settings.RetryDelayInSeconds * 1000, token);
        }
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
    public async Task<bool> PostStatusAsync(string accessid, string saletext)
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

            using HttpClient httpClient = _httpClientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient.PostAsync(new Uri(_settings.EndpointUrl), body);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Successfully pushed status for {AccessId}", accessid);
            _logger.LogDebug("Successfully pushed status {Status}", saletext);
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

    private string ProductInfo(ProductEntity product)
        => string.Create(_formatProvider, $"{product.Brand} - {product.Type.Name}<br/>{product.Description} - {product.Price:C}");
}
