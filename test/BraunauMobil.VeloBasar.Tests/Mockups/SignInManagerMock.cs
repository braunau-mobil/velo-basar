using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class SignInManagerMock
    : SignInManager<IdentityUser>
{
    public SignInManagerMock()
        : base(
            new UserManagerMock(),
            A.Fake<IHttpContextAccessor>(),
            A.Fake<IUserClaimsPrincipalFactory<IdentityUser>>(),
            A.Fake<IOptions<IdentityOptions>>(),
            A.Fake<ILogger<SignInManager<IdentityUser>>>(),
            A.Fake<IAuthenticationSchemeProvider>(),
            A.Fake<IUserConfirmation<IdentityUser>>())
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
