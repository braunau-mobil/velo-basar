using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using Serilog;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Printing;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using System.Linq;

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

            RegisterServices(services);
            services.AddHttpContextAccessor();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            Log.Information("Configure database context");
            services.AddDbContext<VeloRepository>(options =>
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
                .AddEntityFrameworkStores<VeloRepository>();

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
                .AddRazorPages()
                .AddViewLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                });

            Log.Information("Configure services done");
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddScoped<IBasarContext, BasarContext>();
            services.AddScoped<IBrandContext, BrandContext>();
            services.AddScoped<IColorProvider, ColorProvider>();
            services.AddScoped<ICountryContext, CountryContext>();
            services.AddScoped<IDataGeneratorContext, DataGeneratorContext>();
            services.AddScoped<IFileStoreContext, FileStoreContext>();
            services.AddScoped<INumberContext, NumberContext>();
            services.AddScoped<IProductContext, ProductContext>();
            services.AddScoped<IProductTypeContext, ProductTypeContext>();
            services.AddScoped<IStatusPushService, WordPressStatusPushService>();
            services.AddScoped<ISellerContext, SellerContext>();            
            services.AddScoped<ISettingsContext, SettingsContext>();
            services.AddScoped<ISetupContext, SetupContext>();
            services.AddScoped<IStatisticContext, StatisticContext>();
            services.AddScoped<ITokenProvider, SimpleTokenProvider>();
            services.AddScoped<ITransactionContext, TransactionContext>();
            services.AddScoped<IZipMapContext, ZipMapContext>();

            services.AddScoped<IPrintService, PdfPrintService>();
            services.AddScoped<IVeloContext, DefaultVeloContext>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

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

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });


            MigrateDatabase(serviceProvider);
            
            Log.Information("Configure done");
        }

        private static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            Log.Information("Migrate database");

            var db = serviceProvider.GetRequiredService<VeloRepository>();

            if (!db.IsInitialized())
            {
                Log.Information("Database is not initialized, no need for migration");
                return;
            }

            db.Database.Migrate();

            EnsureProductLables(serviceProvider, db);
        }

        private static void EnsureProductLables(IServiceProvider serviceProvider, VeloRepository db)
        {
            Log.Information("Ensuring product labels");

            var fileStoreContext = serviceProvider.GetRequiredService<IFileStoreContext>();
            var settingsContext = serviceProvider.GetRequiredService<ISettingsContext>();
            var printSettings = settingsContext.GetPrintSettingsAsync().Result;
            foreach (var product in db.Products.Where(p => p.LabelId == 0).IncludeAll().ToArrayAsync().Result)
            {
                product.LabelId = fileStoreContext.CreateProductLabelAsync(product, printSettings).Result;
                db.Attach(product).State = EntityState.Modified;

                Log.Verbose("Created label for product id: {productId}", product.Id);
            }
            db.SaveChanges();
        }
    }
}
