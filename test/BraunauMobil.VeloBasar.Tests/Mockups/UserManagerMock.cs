using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class UserManagerMock
    : UserManager<IdentityUser>
{
    private static IOptions<IdentityOptions> CreateOptions()
    {
        IdentityOptions options = new();
        VeloServiceExtensions.ConfigureVeloIdentity(options);
        return Microsoft.Extensions.Options.Options.Create(options);
    }

    public UserManagerMock()
        : base(
            A.Fake<IUserStoreMock<IdentityUser>>(),
            CreateOptions(),
            new PasswordHasher<IdentityUser>(),
            Enumerable.Empty<IUserValidator<IdentityUser>>(),
            Enumerable.Empty<IPasswordValidator<IdentityUser>>(),
            A.Fake<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            A.Fake<IServiceProvider>(),
            A.Fake<ILogger<UserManager<IdentityUser>>>())
    {
    }

    public UserManagerMock(DbContext db)
        : base(new UserStore<IdentityUser>(db),
            CreateOptions(),
            new PasswordHasher<IdentityUser>(),
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
