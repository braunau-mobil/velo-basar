using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class ResponseCookiesFeatureWrapper
    : IResponseCookiesFeature
{
    public ResponseCookiesFeatureWrapper(IResponseCookies cookies)
    {
        Cookies = cookies ?? throw new ArgumentNullException(nameof(cookies));
    }

    public IResponseCookies Cookies { get; }
}
