using AngleSharp;
using AngleSharp.Io;
using System.Net.Http.Headers;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public static class HttpResponseMessageExtensions
{
    public static async Task<IHtmlDocument> GetDocumentAsync(this HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);

        string content = await response.Content.ReadAsStringAsync();
        var document = await BrowsingContext.New()
            .OpenAsync(ResponseFactory, CancellationToken.None);
        return (IHtmlDocument)document;

        void ResponseFactory(VirtualResponse htmlResponse)
        {
            htmlResponse
                .Address(response.RequestMessage!.RequestUri)
                .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);

            htmlResponse.Content(content);

            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }
}
