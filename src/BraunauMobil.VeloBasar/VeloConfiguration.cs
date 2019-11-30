using Microsoft.Extensions.Configuration;
using System;

namespace BraunauMobil.VeloBasar
{
    public static class VeloConfiguration
    {
        public static bool DevToolsEnabled(this IConfiguration configuration) => configuration.GetValue<bool>("DevToolsEnabled");
        public static string GetDatabaseType(this IConfiguration configuration) => configuration.GetValue<string>("DatabaseType");
        public static bool UseSqlServer(this IConfiguration configuration) => string.Compare(configuration.GetDatabaseType(), "sqlserver", StringComparison.InvariantCultureIgnoreCase) == 0;
    }
}
