using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace BraunauMobil.VeloBasar.Tests.Cookies;

public class TestBase
{
	public TestBase()
	{
		FeatureCollection features = new();

		IResponseCookiesFeature responseCookiesFeature = X.StrictFake<IResponseCookiesFeature>();
        A.CallTo(() => responseCookiesFeature.Cookies).Returns(ResponseCookies);
        features[typeof(IResponseCookiesFeature)] = responseCookiesFeature;

		IRequestCookiesFeature requestCookiesFeature = X.StrictFake<IRequestCookiesFeature>();
		A.CallTo(() => requestCookiesFeature.Cookies).Returns(RequestCookies);
		features[typeof(IRequestCookiesFeature)] = requestCookiesFeature;

		HttpContext = new DefaultHttpContext(features);

		A.CallTo(() => HttpContextAccessor.HttpContext).Returns(HttpContext);
    }

	public IRequestCookieCollection RequestCookies { get; } = X.StrictFake<IRequestCookieCollection>();

	public IResponseCookies ResponseCookies { get; } = X.StrictFake<IResponseCookies>();

	public DefaultHttpContext HttpContext { get; }

    public IHttpContextAccessor HttpContextAccessor { get; } = X.StrictFake<IHttpContextAccessor>();
}

