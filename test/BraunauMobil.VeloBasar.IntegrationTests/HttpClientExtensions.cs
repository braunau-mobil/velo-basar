using AngleSharp.Dom;
using AngleSharp.Io;
using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public static class HttpClientExtensions
{
    public static async Task<IHtmlDocument> GetDocumentAsync(this HttpClient client, string requestUri)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(requestUri);

        HttpResponseMessage response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        return await response.GetDocumentAsync();
    }

    public static async Task<IHtmlDocument> GetDocumentAsync(this HttpClient client, HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(request);

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.GetDocumentAsync();
    }

    public static async Task<AcceptanceDocumentModel> GetAcceptanceDocumentAsync(this HttpClient client, string requestUri)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(requestUri);

        return await client.GetJsonDocumentAsync<AcceptanceDocumentModel>(requestUri);
    }

    public static async Task<SaleDocumentModel> GetSaleDocumentAsync(this HttpClient client, string requestUri)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(requestUri);

        return await client.GetJsonDocumentAsync<SaleDocumentModel>(requestUri);
    }

    public static async Task<SettlementDocumentModel> GetSettlementDocumentAsync(this HttpClient client, string requestUri)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(requestUri);

        return await client.GetJsonDocumentAsync<SettlementDocumentModel>(requestUri);
    }

    public static async Task<TModel> GetJsonDocumentAsync<TModel>(this HttpClient client, string requestUri)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(requestUri);

        HttpResponseMessage response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/pdf");
        string json = await response.Content.ReadAsStringAsync();

        return json.DeserializeFromJson<TModel>();
    }

    public static async Task<IHtmlDocument> NavigateMenuAsync(this HttpClient client, string menuEntryText)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(menuEntryText);

        IHtmlDocument indexDocument = await client.GetDocumentAsync("");
        return await client.NavigateMenuAsync(indexDocument, menuEntryText);
    }

    public static async Task<IHtmlDocument> NavigateMenuAsync(this HttpClient client, IHtmlDocument menuDocument, string menuEntryText)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(menuDocument);
        ArgumentNullException.ThrowIfNull(menuEntryText);

        IHtmlAnchorElement menuEntryAnchor = menuDocument.QueryAnchorByText(menuEntryText);
        return await client.GetDocumentAsync(menuEntryAnchor.Href);
    }
    
    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, IHtmlFormElement form)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(form);

        DocumentRequest? request = form.GetSubmission();
        Uri target = (Uri)request!.Target;

        return await client.SendFormAsync(request, target);
    }

    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, IHtmlFormElement form, IEnumerable<KeyValuePair<string, object>> formValues)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(formValues);

        form.SetFormValues(formValues);        
        return await client.SendFormAsync(form);
    }

    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, IHtmlFormElement form, IHtmlElement button)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(button);

        DocumentRequest? request = form.GetSubmission(button);
        Uri target = (Uri)request!.Target;
        string? formaction = button.GetAttribute("formaction");
        if (formaction is not null)
        {
            target = new Uri(formaction, UriKind.Relative);
        }

        return await client.SendFormAsync(request, target);
    }

    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, IHtmlFormElement form, IHtmlElement button, IEnumerable<KeyValuePair<string, object>> formValues)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(button);
        ArgumentNullException.ThrowIfNull(formValues);

        form.SetFormValues(formValues);

        return await client.SendFormAsync(form, button);
    }

    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, DocumentRequest request, Uri target)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(target);
        
        HttpRequestMessage submission = new(new System.Net.Http.HttpMethod(request.Method.ToString()), target)
        {
            Content = new StreamContent(request.Body)
        };

        foreach (var (key, value) in request.Headers)
        {
            submission.Headers.TryAddWithoutValidation(key, value);
            submission.Content.Headers.TryAddWithoutValidation(key, value);
        }

        return await client.GetDocumentAsync(submission);
    }
}
