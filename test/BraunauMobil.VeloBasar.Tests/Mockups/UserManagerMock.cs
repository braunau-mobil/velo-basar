using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class UserManagerMock
    : UserManager<IdentityUser>
{
    public UserManagerMock()
        : base(
            A.Fake<IUserStoreMock<IdentityUser>>(),
            A.Fake<IOptions<IdentityOptions>>(),
            A.Fake<IPasswordHasher<IdentityUser>>(),
            Enumerable.Empty<IUserValidator<IdentityUser>>(),
            Enumerable.Empty<IPasswordValidator<IdentityUser>>(),
            A.Fake<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            A.Fake<IServiceProvider>(),
            A.Fake<ILogger<UserManager<IdentityUser>>>())
    {
    }
}

public interface IUserStoreMock<TUser>
    : IUserStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : IdentityUser
{
}
