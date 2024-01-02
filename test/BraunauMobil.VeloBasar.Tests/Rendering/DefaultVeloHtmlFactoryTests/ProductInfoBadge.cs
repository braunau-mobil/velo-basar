using Microsoft.AspNetCore.Html;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class ProductInfoBadge
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public void ShouldReturnCorrectHtml(ProductEntity product)
    {
        //  Arrange
        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;
        product.DonateIfNotSold = true;

        //  Act
        IHtmlContent result = Sut.ProductInfoBadges(product);

        //  Assert
        result.Should().BeHtml("""<span class="badge rounded-pill text-bg-success">VeloBasar_Available</span><br><span class="badge rounded-pill text-bg-info">VeloBasar_Donateable</span>""");
    }
}
