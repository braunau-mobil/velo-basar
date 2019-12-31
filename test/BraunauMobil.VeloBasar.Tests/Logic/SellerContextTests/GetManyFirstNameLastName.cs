using AutoFixture;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.SellerContextTests
{
    public class GetManyFirstNameLastName : TestWithServicesAndDb
    {
        public GetManyFirstNameLastName()
        {
            AddLocalization();
            AddLogic();
        }

        [Fact]
        public async Task CaseSensitivness()
        {
            await RunWithServiesAndDb(async serviceProvider =>
            {
                var fixture = new Fixture();

                var setupContext = serviceProvider.GetRequiredService<ISetupContext>();
                await setupContext.InitializeDatabaseAsync(new VeloBasar.Models.InitializationConfiguration());

                var basarContext = serviceProvider.GetRequiredService<IBasarContext>();
                var basar = await basarContext.CreateAsync(fixture.Create<Basar>());

                var sellerContecxt = serviceProvider.GetRequiredService<ISellerContext>();
                var seller = await sellerContecxt.CreateAsync(fixture.Build<Seller>()
                    .With(s => s.FirstName, "Test").Create());

                var sellers = await sellerContecxt.GetMany("test", null).ToArrayAsync();
                Assert.Single(sellers);
            });
        }
    }
}
