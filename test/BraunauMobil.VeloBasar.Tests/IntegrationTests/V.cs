using BraunauMobil.VeloBasar.Controllers;
using Xan.AspNetCore.Mvc.Crud;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public static class V
{
    public const string AdminUserEMail = "dev@xaka.eu";

    public static async Task Init(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        await services.Do<SetupController>(async setup =>
        {
            InitializationConfiguration config = new()
            {
                AdminUserEMail = AdminUserEMail,
                GenerateCountries = true,
                GenerateProductTypes = true,
                GenerateZipCodes = true,
            };
            await setup.InitialSetupConfirmed(config);
        });
    }

    public static class FirstBasar
    {
        public const string Name = "1. Basar";
        public const int ProductCommissionPercentage = 10;
        public const decimal ProductCommission = 0.1m;
        public static readonly DateTime Date = new(2063, 04, 05);
        public const string Location = "Braunau";

        public static async Task Init(IServiceProvider services)
        {
            ArgumentNullException.ThrowIfNull(services);

            await services.Do<CrudController<BasarEntity>>(async controller =>
            {
                BasarEntity basar = new ()
                {
                    Name = Name,
                    ProductCommissionPercentage = ProductCommissionPercentage,
                    Date = Date,
                    Location = Location,
                };
                await controller.Create(basar);
            });
        }
    }

    public static class Countries
    {
        public const string Austria = "Österreich";
    }
    public static class ProductTypes
    {
        public const string SteelSteed = "Stahlross";
    }
    public static class ZipCodes
    {
        public const string Braunau = "5280";
    }
}
