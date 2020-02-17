using BraunauMobil.VeloBasar.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Data.DataGeneratorContextTests
{
    public class GenerateAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task GenerateManyBasars()
        {
            await RunOnEmptyDb(async db =>
            {
                await DataGeneratorContext.GenerateAsync(new DataGeneratorConfiguration
                {
                    AdminUserEMail = "dev@xaka.eu",
                    BasarCount = 100,
                    FirstBasarDate = new DateTime(2063, 4, 5),
                    GenerateBrands = true,
                    GenerateCountries = true,
                    GenerateProductTypes = true,
                    MaxAcceptancesPerSeller = 1,
                    MaxSellers = 1,
                    MeanPrice = 1,
                    MeanProductsPerSeller = 1,
                    MinAcceptancesPerSeller = 1,
                    MinSellers = 1,
                    StdDevPrice = 1,
                    StdDevProductsPerSeller = 1
                });
            });
        }
    }
}
