using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace BraunauMobil.VeloBasar.Tests.Cookies;

public class TestBase
{
	public TestBase()
	{
		FeatureCollection features = new();

		Mock<IResponseCookiesFeature> responseCookiesFeature = new();
        responseCookiesFeature.Setup(_ => _.Cookies)
            .Returns(ResponseCookies.Object);
        features[typeof(IResponseCookiesFeature)] = responseCookiesFeature.Object;

		Mock<IRequestCookiesFeature> requestCookiesFeature = new();
		requestCookiesFeature.Setup(_ => _.Cookies)
			.Returns(RequestCookies.Object);
		features[typeof(IRequestCookiesFeature)] = requestCookiesFeature.Object;

		HttpContext = new DefaultHttpContext(features);

		HttpContextAccessor.Setup(_ => _.HttpContext)
			.Returns(HttpContext);
    }

	public void VerifyNoOtherCalls()
	{
		RequestCookies.VerifyNoOtherCalls();
		ResponseCookies.VerifyNoOtherCalls();
	}

	public Mock<IRequestCookieCollection> RequestCookies { get; } = new();

	public Mock<IResponseCookies> ResponseCookies { get; } = new();

	public DefaultHttpContext HttpContext { get; }

    public Mock<IHttpContextAccessor> HttpContextAccessor { get; } = new ();
}

