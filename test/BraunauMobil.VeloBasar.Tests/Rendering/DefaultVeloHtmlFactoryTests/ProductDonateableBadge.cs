using Microsoft.AspNetCore.Html;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class ProductDonateableBadge
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public void DoNotDonate_ShouldReturnEmptyHtmlString(ProductEntity product)
    {
        //  Arrange
        product.DonateIfNotSold = false;

        //  Act
        IHtmlContent result = Sut.ProductDonateableBadge(product);

        //  Assert
        result.Should().BeHtml("");
    }

    [Theory]
    [VeloAutoData]
    public void DoDonate_ShouldReturnBadge(ProductEntity product)
    {
        //  Arrange
        product.DonateIfNotSold = true;

        //  Act
        IHtmlContent result = Sut.ProductDonateableBadge(product);

        //  Assert
        result.Should().BeHtml("""<span class="badge rounded-pill text-bg-info">VeloBasar_Donateable</span>""");
    }
}
