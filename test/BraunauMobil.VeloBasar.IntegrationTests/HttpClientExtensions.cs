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

    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, IHtmlFormElement form, IHtmlElement button)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(button);
        ArgumentNullException.ThrowIfNull(form);

        return await client.SendFormAsync(form, button, []);
    }

    public static async Task<IHtmlDocument> SendFormAsync(this HttpClient client, IHtmlFormElement form, IHtmlElement button, IEnumerable<KeyValuePair<string, object>> formValues)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(button);
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(formValues);
        
        foreach (var (key, value) in formValues)
        {
            IElement? formElement = form[key];
            if (formElement is IHtmlInputElement input)
            {
                if (input.Type == "checkbox")
                {
                    input.IsChecked = value.Should().BeOfType<bool>().Subject;
                }
                else
                {
                    input.Value = value.ToString() ?? throw new NullReferenceException($"String value for element {key} is null.");
                }
            }
            else if (formElement is IHtmlSelectElement select)
            {
                string selectedValue = value.ToString() ?? throw new NullReferenceException($"String value for element {key} is null.");
                foreach (IHtmlOptionElement option in select.Options)
                {
                    option.IsSelected = option.Value == selectedValue;
                }
            }
            else if (formElement is null)
            {
                throw new InvalidOperationException($"No form element found for value {key}({value}).");
            }
            else
            {
                throw new InvalidOperationException($"Form element for value {key}({value}) has an unsupported type: {formElement.GetType()}");
            }
        }

        DocumentRequest? submit = form.GetSubmission(button);
        Uri target = (Uri)submit!.Target;
        if (button.HasAttribute("formaction"))
        {
            string? formaction = button.GetAttribute("formaction");
            if (formaction is not null)
            {
                target = new Uri(formaction, UriKind.Relative); 
            }
        }
        HttpRequestMessage submission = new(new System.Net.Http.HttpMethod(submit.Method.ToString()), target)
        {
            Content = new StreamContent(submit.Body)
        };

        foreach (var (key, value) in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(key, value);
            submission.Content.Headers.TryAddWithoutValidation(key, value);
        }

        return await client.GetDocumentAsync(submission);
    }
}
