using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.WordPressStatusPushServiceTests;

public class PostStatusAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task EndpointUrlNotSet_ShouldReturnFalse(string accessid, string salestext)
    {
        //  Arrange
        Settings.EndpointUrl = null;

        //  Act
        bool result = await Sut.PostStatusAsync(accessid, salestext);

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task ShouldCallPostAsyncAndReturnTrue(string accessid, string salestext, string apiKey)
    {
        //  Arrange
        Settings.EndpointUrl = "http://localhost";
        Settings.ApiKey = apiKey;

        MyHttpMessageHandler myHttpMessageHandler = new ();
        HttpClient client = new (myHttpMessageHandler);
        A.CallTo(() => HttpClientFactory.CreateClient(Options.DefaultName)).Returns(client);

        //  Act
        bool result = await Sut.PostStatusAsync(accessid, salestext);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            myHttpMessageHandler.Request.Should().NotBeNull();
            myHttpMessageHandler.Request!.RequestUri.Should().Be(new Uri(Settings.EndpointUrl));
            myHttpMessageHandler.Request!.Method.Should().Be(HttpMethod.Post);
            StringContent content = myHttpMessageHandler.Request!.Content.Should().BeOfType<StringContent>().Subject;
            content.Headers.Should().ContainKey("bm-velobasar-api-token").WhoseValue.Should().BeEquivalentTo(new[] { apiKey });
            content.Headers.Should().ContainKey("Content-Type").WhoseValue.Should().BeEquivalentTo(new[] { "application/json; charset=utf-8" });
        }
    }

    private class MyHttpMessageHandler
        : HttpMessageHandler
    {
        public HttpRequestMessage? Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
