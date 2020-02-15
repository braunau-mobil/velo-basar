using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.FileStoreContextTests
{
    public class GetProductLabelsAndCombineToOnePdfAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task MultipleAcceptances()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(new Basar());

                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var firstAcceptance = TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.Build<Product>().Without(p => p.LabelId).CreateMany(1).ToList());
                var secondAceptance = TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.Build<Product>().Without(p => p.LabelId).CreateMany(2).ToList());

                var products = await ProductContext.GetProductsForSeller(basar, seller.Id).ToArrayAsync();

                var lables = await FileStoreContext.GetProductLabelsAndCombineToOnePdfAsync(products);

                PdfAssert.PageCount(3, lables.Data);
            });
        }
    }
}
