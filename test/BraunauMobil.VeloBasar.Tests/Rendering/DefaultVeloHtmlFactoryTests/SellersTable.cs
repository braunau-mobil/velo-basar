using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class SellersTable
    : TestBase
{
    [Fact]
    public void DefaultConfiguration()
    {
        //  Arrange
        IEnumerable<SellerEntity> sellers = Enumerable.Empty<SellerEntity>();

        //  Act
        TableBuilder<SellerEntity> table = Sut.SellersTable(sellers);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("Xan_AspNetCore_Id"),
            column => column.Title.Should().BeHtml("VeloBasar_FirstName"),
            column => column.Title.Should().BeHtml("VeloBasar_LastName"),
            column => column.Title.Should().BeHtml("VeloBasar_Address"),
            column => column.Title.Should().BeHtml("")
        );
    }
}
