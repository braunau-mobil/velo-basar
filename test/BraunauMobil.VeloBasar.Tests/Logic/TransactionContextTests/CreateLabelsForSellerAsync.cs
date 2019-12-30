using AutoFixture;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using iText.Kernel.Pdf;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionContextTests
{
    public class CreateLabelsForSellerAsync : TestWithServicesAndDb
    {
        public CreateLabelsForSellerAsync()
        {
            AddLocalization();
            AddLogic();
        }

        [Fact]
        public async Task MultipleAcceptances()
        {
            await RunWithServiesAndDb(async serviceProvider =>
            {
                var fixture = new Fixture();
                var setupContext = serviceProvider.GetRequiredService<ISetupContext>();
                await setupContext.InitializeDatabaseAsync(new InitializationConfiguration());

                var basarContext = serviceProvider.GetRequiredService<IBasarContext>();
                var basar = await basarContext.CreateAsync(new Basar());

                var sellerContext = serviceProvider.GetRequiredService<ISellerContext>();
                var seller = await sellerContext.CreateAsync(fixture.Create<Seller>());

                var transactionContext = serviceProvider.GetRequiredService<ITransactionContext>();
                var firstAcceptance = transactionContext.AcceptProductsAsync(basar, seller.Id, fixture.Build<Product>().Without(p => p.Label).CreateMany(1).ToList());
                var secondAceptance = transactionContext.AcceptProductsAsync(basar, seller.Id, fixture.Build<Product>().Without(p => p.Label).CreateMany(2).ToList());

                var lables = await transactionContext.CreateLabelsForSellerAsync(basar, seller.Id);

                using var pfdStream = new MemoryStream(lables.Data);
                using var pdfReader = new PdfReader(pfdStream);
                using var pdfDoc = new PdfDocument(pdfReader);
                Assert.Equal(3, pdfDoc.GetNumberOfPages());
            });
        }
    }
}
