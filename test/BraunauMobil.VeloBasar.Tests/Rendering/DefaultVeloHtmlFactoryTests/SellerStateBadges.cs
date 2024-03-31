using Microsoft.AspNetCore.Html;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class SellerStateBadges
    : TestBase
{
    [Theory]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-primary">VeloBasar_NotSettled</span><br><span class="badge rounded-pill text-bg-warning">VeloBasar_OnSiteSingular</span>""", ValueState.NotSettled, SellerSettlementType.OnSite)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-primary">VeloBasar_NotSettled</span><br><span class="badge rounded-pill text-bg-success">VeloBasar_RemoteSingular</span>""", ValueState.NotSettled, SellerSettlementType.Remote)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-secondary">VeloBasar_Settled</span><br><span class="badge rounded-pill text-bg-secondary">VeloBasar_OnSiteSingular</span>""", ValueState.Settled, SellerSettlementType.OnSite)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-secondary">VeloBasar_Settled</span><br><span class="badge rounded-pill text-bg-secondary">VeloBasar_RemoteSingular</span>""", ValueState.Settled, SellerSettlementType.Remote)]
    public void ShouldReturnCorrectHtml(string expectedHtml, ValueState valueState, SellerSettlementType settlementType, SellerEntity seller)
    {
        //  Arrange
        seller.ValueState = valueState;
        seller.SettlementType = settlementType;

        //  Act
        IHtmlContent result = Sut.SellerStateBadges(seller);

        //  Assert
        result.Should().BeHtml(expectedHtml);
    }
}
