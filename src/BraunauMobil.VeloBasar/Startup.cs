using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using Serilog;

namespace BraunauMobil.VeloBasar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Configure services");

            services.AddScoped<IVeloContext, DefaultVeloContext>();
            services.AddHttpContextAccessor();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            Log.Information("Configure database context");
            services.AddDbContext<VeloBasarContext>(options =>
            {
                if (Configuration.UseSqlServer())
                {
                    options.UseSqlServer(Configuration.GetConnectionString("VeloBasarContextSqlServer"));
                }
                else
                {
                    options.UseNpgsql(Configuration.GetConnectionString("VeloBasarContextPostgresql"));
                }
            });

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<VeloBasarContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services
                .AddMvc()
                .AddViewLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Log.Information("Configure services done");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Information("Configure");

            var cultureInfo = CultureInfo.GetCultureInfo("de-AT");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();

            Log.Information("Configure done");
        }
    }
}
