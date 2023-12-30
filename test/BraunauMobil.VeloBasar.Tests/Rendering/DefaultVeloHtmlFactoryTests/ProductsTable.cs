using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class ProductsTable
    : TestBase
{
    [Fact]
    public void DefaultConfiguration()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        TableBuilder<ProductEntity> table = Sut.ProductsTable(products);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("VeloBasar_Brand"),
            column => column.Title.Should().BeHtml("VeloBasar_Type"),
            column => column.Title.Should().BeHtml("VeloBasar_Color"),
            column => column.Title.Should().BeHtml("VeloBasar_FrameNumber"),
            column => column.Title.Should().BeHtml("VeloBasar_Description"),
            column => column.Title.Should().BeHtml("VeloBasar_TireSize"),
            column => column.Title.Should().BeHtml("VeloBasar_Price")
        );
    }

    [Fact]
    public void ShowId()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        TableBuilder<ProductEntity> table = Sut.ProductsTable(products, showId: true);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("Xan_AspNetCore_Id"),
            column => column.Title.Should().BeHtml("VeloBasar_Brand"),
            column => column.Title.Should().BeHtml("VeloBasar_Type"),
            column => column.Title.Should().BeHtml("VeloBasar_Color"),
            column => column.Title.Should().BeHtml("VeloBasar_FrameNumber"),
            column => column.Title.Should().BeHtml("VeloBasar_Description"),
            column => column.Title.Should().BeHtml("VeloBasar_TireSize"),
            column => column.Title.Should().BeHtml("VeloBasar_Price")
        );
    }

    [Fact]
    public void ShowState()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        TableBuilder<ProductEntity> table = Sut.ProductsTable(products, showState: true);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("VeloBasar_Brand"),
            column => column.Title.Should().BeHtml("VeloBasar_Type"),
            column => column.Title.Should().BeHtml("VeloBasar_Color"),
            column => column.Title.Should().BeHtml("VeloBasar_FrameNumber"),
            column => column.Title.Should().BeHtml("VeloBasar_Description"),
            column => column.Title.Should().BeHtml("VeloBasar_TireSize"),
            column => column.Title.Should().BeHtml("VeloBasar_Price"),
            column => column.Title.Should().BeHtml("")
        );
    }

    [Fact]
    public void ShowIdAndState()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        TableBuilder<ProductEntity> table = Sut.ProductsTable(products, showId: true, showState: true);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("Xan_AspNetCore_Id"),
            column => column.Title.Should().BeHtml("VeloBasar_Brand"),
            column => column.Title.Should().BeHtml("VeloBasar_Type"),
            column => column.Title.Should().BeHtml("VeloBasar_Color"),
            column => column.Title.Should().BeHtml("VeloBasar_FrameNumber"),
            column => column.Title.Should().BeHtml("VeloBasar_Description"),
            column => column.Title.Should().BeHtml("VeloBasar_TireSize"),
            column => column.Title.Should().BeHtml("VeloBasar_Price"),
            column => column.Title.Should().BeHtml("")
        );
    }
}
