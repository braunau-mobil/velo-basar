using BraunauMobil.VeloBasar.Parameters;

namespace BraunauMobil.VeloBasar.Tests.Parameters;

public class TransactionListParameterTests
{
    [Theory]
    [VeloAutoData]
    public void CopyCtor(TransactionListParameter source)
    {
        //  Arrange

        //  Act
        TransactionListParameter target = new (source);

        //  Assert
        target.Should().BeEquivalentTo(source);
    }
}
