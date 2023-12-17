using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class UserManagerMock
    : UserManager<IdentityUser>
{
    public UserManagerMock()
        : base(
            Mock.Of<IUserStoreMock<IdentityUser>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<IdentityUser>>(),
            Enumerable.Empty<IUserValidator<IdentityUser>>(),
            Enumerable.Empty<IPasswordValidator<IdentityUser>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<IdentityUser>>>())
    {
    }
}

public interface IUserStoreMock<TUser>
    : IUserStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : IdentityUser
{
}
