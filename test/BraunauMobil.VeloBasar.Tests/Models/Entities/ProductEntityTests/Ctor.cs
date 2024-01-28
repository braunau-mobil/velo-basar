namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        ProductEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Brand.Should().BeNull();
            sut.Color.Should().BeNull();
            sut.CreatedAt.Should().Be(DateTime.MinValue);
            sut.Description.Should().BeNull();
            sut.DonateIfNotSold.Should().BeFalse();
            sut.FrameNumber.Should().BeNull();
            sut.Id.Should().Be(0);
            sut.Price.Should().Be(0);
            sut.Session.Should().BeNull();
            sut.SessionId.Should().Be(0);
            sut.StorageState.Should().Be(StorageState.NotAccepted);
            sut.TireSize.Should().BeNull();
            sut.Type.Should().BeNull();
            sut.TypeId.Should().Be(0);
            sut.UpdatedAt.Should().Be(DateTime.MinValue);
            sut.ValueState.Should().Be(ValueState.NotSettled);
        }
    }
}
