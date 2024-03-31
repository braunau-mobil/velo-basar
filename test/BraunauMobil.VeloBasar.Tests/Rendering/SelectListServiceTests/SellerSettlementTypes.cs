using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class SellerSettlementTypes
    : TestBase
{
    private readonly List<Action<SelectListItem>> _elementInspectors;

    public SellerSettlementTypes()
    {
        _elementInspectors = new List<Action<SelectListItem>>()
        {
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_OnSiteSingular");
                item.Value.Should().Be("OnSite");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_RemoteSingular");
                item.Value.Should().Be("Remote");
            },
        };
    }

    [Fact]
    public void DoNotIncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.SellerSettlementTypes();

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }

    [Fact]
    public void IncludeAll()
    {
        //  Arrange
        _elementInspectors.Insert(0, item =>
        {
            item.Disabled.Should().BeFalse();
            item.Group.Should().BeNull();
            item.Selected.Should().BeFalse();
            item.Text.Should().Be("VeloBasar_AllSellerSettlementTypes");
            item.Value.Should().Be("");
        });

        // Act
        SelectList result = Sut.SellerSettlementTypes(includeAll: true);

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }
}
