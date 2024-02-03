using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Xan.Extensions.Tasks;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.WordPressStatusPushServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new WordPressStatusPushService(Options.Create(Settings), new StringLocalizerMock<SharedResources>(), BackgroundTaskQueue, A.Fake<ILogger<WordPressStatusPushService>>(), Db, HttpClientFactory);
    }

    protected IBackgroundTaskQueue BackgroundTaskQueue { get; } = X.StrictFake<IBackgroundTaskQueue>();

    protected IHttpClientFactory HttpClientFactory { get; } = X.StrictFake<IHttpClientFactory>();

    protected WordPressStatusPushService Sut { get; }

    protected WordPressStatusPushSettings Settings { get; } = new WordPressStatusPushSettings();
}
