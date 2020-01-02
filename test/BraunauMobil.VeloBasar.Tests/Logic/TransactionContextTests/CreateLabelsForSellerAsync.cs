using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using iText.Kernel.Pdf;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionContextTests
{
    public class CreateLabelsForSellerAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task MultipleAcceptances()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(new Basar());

                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var firstAcceptance = TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.Build<Product>().Without(p => p.Label).CreateMany(1).ToList());
                var secondAceptance = TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.Build<Product>().Without(p => p.Label).CreateMany(2).ToList());

                var lables = await TransactionContext.CreateLabelsForSellerAsync(basar, seller.Id);

                using var pfdStream = new MemoryStream(lables.Data);
                using var pdfReader = new PdfReader(pfdStream);
                using var pdfDoc = new PdfDocument(pdfReader);
                Assert.Equal(3, pdfDoc.GetNumberOfPages());
            });
        }
    }
}
