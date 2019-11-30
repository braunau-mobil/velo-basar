using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore;
using Serilog;
using Serilog.Events;

namespace BraunauMobil.VeloBasar
{
    public static class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("velo-basar.log")
                .WriteTo.Debug()
                .CreateLogger();
#else
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File("velo-basar.log")
                .WriteTo.Console()
                .CreateLogger();
#endif

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
