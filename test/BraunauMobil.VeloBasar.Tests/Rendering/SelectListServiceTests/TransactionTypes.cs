using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class TransactionTypes
    : TestBase
{
    private readonly List<Action<SelectListItem>> _elementInspectors;

    public TransactionTypes()
    {
        _elementInspectors = new List<Action<SelectListItem>>()
        {
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_AcceptanceSingular");
                item.Value.Should().Be("Acceptance");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_SaleSingular");
                item.Value.Should().Be("Sale");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_SettlementSingular");
                item.Value.Should().Be("Settlement");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_CancellationSingular");
                item.Value.Should().Be("Cancellation");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_LockSingular");
                item.Value.Should().Be("Lock");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_SetLostSingular");
                item.Value.Should().Be("SetLost");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_UnlockSingular");
                item.Value.Should().Be("Unlock");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_UnsettlementSingular");
                item.Value.Should().Be("Unsettlement");
            }
        };
    }

    [Fact]
    public void DoNotIncludeAll()
    {
        //  Arrange

        // Act
        SelectList result = Sut.TransactionTypes();

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
            item.Text.Should().Be("VeloBasar_AllTransactionTypes");
            item.Value.Should().Be("");
        });

        // Act
        SelectList result = Sut.TransactionTypes(includeAll: true);

        // Assert
        result.Should().SatisfyRespectively(_elementInspectors);
    }
}
