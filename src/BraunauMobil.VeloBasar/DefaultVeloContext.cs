using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar
{
    public class DefaultVeloContext : IVeloContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultVeloContext(VeloBasarContext dbContext, IStringLocalizer<SharedResource> localizer, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            Db = dbContext;
            Localizer = localizer;
            SignInManager = signInManager;
            UserManager = userManager;

            LoadBasar();
        }

        public Basar Basar { get; private set; }
        public VeloBasarContext Db { get; private set; }
        public IStringLocalizer<SharedResource> Localizer { get; private set; }
        public SignInManager<IdentityUser> SignInManager { get; private set; }
        public UserManager<IdentityUser> UserManager { get; private set; }
        public bool IsDebug
        {
#if DEBUG
            get => true;
#else
            get => false;
#endif
        }

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
                    Basar = Db.Basar.Get(basarId.Value);
                    return;
                }
            }

            if (Db.Settings.ActiveBasar != null)
            {
                Basar = Db.Settings.ActiveBasar;
            }
        }
    }
}
