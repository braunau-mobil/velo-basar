using Microsoft.AspNetCore.Html;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class ProductStateBadge
    : TestBase
{
    [Theory]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-light">VeloBasar_Accepting</span>""", StorageState.NotAccepted, ValueState.NotSettled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-success">VeloBasar_Available</span>""", StorageState.Available, ValueState.NotSettled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-secondary">VeloBasar_PickedUp</span>""", StorageState.Available, ValueState.Settled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-warning">VeloBasar_Sold</span>""", StorageState.Sold, ValueState.NotSettled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-secondary">VeloBasar_Settled</span>""", StorageState.Sold, ValueState.Settled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-danger">VeloBasar_Locked</span>""", StorageState.Locked, ValueState.NotSettled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-secondary">VeloBasar_PickedUp</span>""", StorageState.Locked, ValueState.Settled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-danger">VeloBasar_Lost</span>""", StorageState.Lost, ValueState.NotSettled)]
    [VeloInlineAutoData("""<span class="badge rounded-pill text-bg-secondary">VeloBasar_Settled</span>""", StorageState.Lost, ValueState.Settled)]
    public void DefinedState_ShouldReturnCorrectHtml(string expectedHtml, StorageState storageState, ValueState valueState, ProductEntity product)
    {
        //  Arrange
        product.StorageState = storageState;
        product.ValueState = valueState;

        //  Act
        IHtmlContent result = Sut.ProductStateBadge(product);

        //  Assert
        result.Should().BeHtml(expectedHtml);
    }

    [Theory]
    [VeloInlineAutoData(StorageState.NotAccepted, ValueState.Settled)]
    public void UndefinedState_ShouldReturnCorrectHtml(StorageState storageState, ValueState valueState, ProductEntity product)
    {
        //  Arrange
        product.StorageState = storageState;
        product.ValueState = valueState;

        //  Act
        IHtmlContent result = Sut.ProductStateBadge(product);

        //  Assert
        result.Should().BeHtml("""<span class="badge rounded-pill text-bg-danger">VeloBasar_UndefinedProductState</span>""");
    }
}
