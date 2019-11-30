using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar
{
    public class DefaultVeloContext : IVeloContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultVeloContext(VeloBasarContext dbContext, IStringLocalizer<SharedResource> localizer, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;

            Configuration = configuration;
            Db = dbContext;
            Localizer = localizer;
            SignInManager = signInManager;
            UserManager = userManager;

            LoadBasar();
        }

        public Basar Basar { get; private set; }
        public VeloSettings Settings { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public VeloBasarContext Db { get; private set; }
        public IStringLocalizer<SharedResource> Localizer { get; private set; }
        public SignInManager<IdentityUser> SignInManager { get; private set; }
        public UserManager<IdentityUser> UserManager { get; private set; }

        public async Task SaveSettingsAsync() => await Db.SetBasarSettingsAsync(Settings);

        private void LoadBasar()
        {
            if (!Db.IsInitialized())
            {
                return;
            }

            if (SignInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {
                var basarId = _httpContextAccessor.HttpContext.Request.Cookies.GetBasarId();
                if(basarId.HasValue && Db.Basar.Exists(basarId.Value))
                {
                    Basar = Db.Basar.GetAsNonTracking(basarId.Value);
                    return;
                }
            }

            Settings = Db.GetVeloSettings();
            
            if (Settings.ActiveBasarId != null)
            {
                Basar = Db.Basar.GetAsNonTracking(Settings.ActiveBasarId.Value);
            }
        }
    }
}
