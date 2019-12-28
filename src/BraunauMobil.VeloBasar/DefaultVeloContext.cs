using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar
{
    public class DefaultVeloContext : IVeloContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasarContext _basarContext;
        private readonly ISettingsContext _settingsContext;
        private readonly VeloRepository _db;

        public DefaultVeloContext(VeloRepository db, IStringLocalizer<SharedResource> localizer, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IBasarContext basarContext, ISettingsContext settingsContext)
        {
            _httpContextAccessor = httpContextAccessor;

            _db = db;
            _basarContext = basarContext;
            _settingsContext = settingsContext;
            Configuration = configuration;
            Localizer = localizer;
            SignInManager = signInManager;
            UserManager = userManager;

            Load();
        }

        public Basar Basar { get; private set; }
        public VeloSettings Settings { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IStringLocalizer<SharedResource> Localizer { get; private set; }
        public SignInManager<IdentityUser> SignInManager { get; private set; }
        public UserManager<IdentityUser> UserManager { get; private set; }

        public bool IsInitialized() => _db.IsInitialized();
        private void Load()
        {
            if (!_db.IsInitialized())
            {
                return;
            }

            LoadBasar();
        }
        private void LoadBasar()
        {
            if (SignInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {
                var basarId = _httpContextAccessor.HttpContext.Request.Cookies.GetBasarId();
                if(basarId.HasValue && _basarContext.Exists(basarId.Value))
                {
                    Basar = _basarContext.GetSingle(basarId.Value);
                    return;
                }
            }

            var settings = _settingsContext.GetSettings();
            if (settings.ActiveBasarId != null)
            {
                Basar = _basarContext.GetSingle(settings.ActiveBasarId.Value);
            }
        }
    }
}
