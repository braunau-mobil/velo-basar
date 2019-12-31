using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar
{
    public interface IVeloContext
    {
        Basar Basar { get; }

        IConfiguration Configuration { get; }
        IStringLocalizer<SharedResource> Localizer { get; }
        SignInManager<IdentityUser> SignInManager { get; }
        UserManager<IdentityUser> UserManager { get; }

        bool IsInitialized();
        bool DevToolsEnabled();
    }
}
