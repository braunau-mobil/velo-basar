using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests;

public class SignInManagerMock
    : SignInManager<IdentityUser>
{
    public SignInManagerMock()
        : base(
            new UserManagerMock(),
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<ILogger<SignInManager<IdentityUser>>>(),
            Mock.Of<IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<IdentityUser>>())
    {
    }

    public Func<ClaimsPrincipal, bool>? IsSignedInMock { get; set; }

    public override bool IsSignedIn(ClaimsPrincipal principal)
    {
        if (IsSignedInMock is not null)
        {
            return IsSignedInMock(principal);
        }
        return base.IsSignedIn(principal);
    }
}
